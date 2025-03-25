namespace GamaEdtech.Application.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using EntityFramework.Exceptions.Common;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Core.Extensions.Linq;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.Service;
    using GamaEdtech.Data.Dto.School;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;

    using MetadataExtractor;
    using MetadataExtractor.Formats.Exif;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using NetTopologySuite;
    using NetTopologySuite.Geometries;

    using static GamaEdtech.Common.Core.Constants;

    public class SchoolService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<FileService>> localizer
        , Lazy<ILogger<FileService>> logger, Lazy<IFileService> fileService)
        : LocalizableServiceBase<FileService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), ISchoolService
    {
        public async Task<ResultData<ListDataSource<SchoolsDto>>> GetSchoolsAsync(ListRequestDto<School>? requestDto = null)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var result = await uow.GetRepository<School, int>().GetManyQueryable(requestDto?.Specification).FilterListAsync(requestDto?.PagingDto);
                var users = await result.List.Select(t => new SchoolsDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    LocalName = t.LocalName,
                }).ToListAsync();
                return new(OperationResult.Succeeded) { Data = new() { List = users, TotalRecordsCount = result.TotalRecordsCount } };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<ListDataSource<SchoolInfoDto>>> GetSchoolsListAsync(ListRequestDto<School>? requestDto = null, Point? point = null)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var lst = uow.GetRepository<School, int>().GetManyQueryable(requestDto?.Specification).Select(t => new
                {
                    t.Id,
                    t.Name,
                    t.LastModifyDate,
                    t.CreationDate,
                    t.WebSite,
                    t.Email,
                    t.PhoneNumber,
                    t.Coordinates,
                    Score = t.Comments != null ? t.Comments.Average(c => c.AverageRate) : (double?)null,
                    CityTitle = t.City == null ? "" : t.City.Title,
                    CountryTitle = t.Country == null ? "" : t.Country.Title,
                    StateTitle = t.State == null ? "" : t.State.Title,
                    Distance = t.Coordinates!.Distance(point),
                }).OrderBy(t => t.Distance);

                int? total = requestDto?.PagingDto?.PageFilter?.ReturnTotalRecordsCount == true ? await lst.CountAsync() : null;
                var data = await (requestDto?.PagingDto?.PageFilter is null ? lst : lst.Skip(requestDto.PagingDto.PageFilter.Skip).Take(requestDto.PagingDto.PageFilter.Size)).ToListAsync();

                var result = data.Select(t => new SchoolInfoDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    LastModifyDate = t.LastModifyDate ?? t.CreationDate,
                    HasWebSite = !string.IsNullOrEmpty(t.WebSite),
                    HasEmail = !string.IsNullOrEmpty(t.Email),
                    HasPhoneNumber = !string.IsNullOrEmpty(t.PhoneNumber),
                    Coordinates = t.Coordinates,
                    Score = t.Score,
                    CityTitle = t.CityTitle,
                    CountryTitle = t.CountryTitle,
                    StateTitle = t.StateTitle,
                    Distance = point != null && t.Coordinates != null ? t.Coordinates.Distance(point) : null,
                });
                return new(OperationResult.Succeeded) { Data = new() { List = result, TotalRecordsCount = total } };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<SchoolDto>> GetSchoolAsync([NotNull] ISpecification<School> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var school = await uow.GetRepository<School, int>().GetManyQueryable(specification).Select(t => new SchoolDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    LocalName = t.LocalName,
                    Address = t.Address,
                    LocalAddress = t.LocalAddress,
                    Coordinates = t.Coordinates,
                    SchoolType = t.SchoolType,
                    ZipCode = t.ZipCode,
                    CityId = t.CityId,
                    CityTitle = t.City != null ? t.City.Title : "",
                    CountryId = t.CountryId,
                    CountryTitle = t.Country != null ? t.Country.Title : "",
                    StateId = t.StateId,
                    StateTitle = t.State != null ? t.State.Title : "",
                    Facilities = t.Facilities,
                    WebSite = t.WebSite,
                    FaxNumber = t.FaxNumber,
                    PhoneNumber = t.PhoneNumber,
                    Email = t.Email,
                    Quarter = t.Quarter,
                    OsmId = t.OsmId,
                }).FirstOrDefaultAsync();

                return school is null
                    ? new(OperationResult.NotFound)
                    {
                        Errors = [new() { Message = Localizer.Value["SchoolNotFound"] },],
                    }
                    : new(OperationResult.Succeeded) { Data = school };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<int>> ManageSchoolAsync([NotNull] ManageSchoolRequestDto requestDto)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<School, int>();
                School? school = null;

                if (requestDto.Id.HasValue)
                {
                    school = await repository.GetAsync(requestDto.Id.Value);
                    if (school is null)
                    {
                        return new(OperationResult.NotFound)
                        {
                            Errors = [new() { Message = Localizer.Value["SchoolNotFound"] },],
                        };
                    }

                    school.Name = requestDto.Name;
                    school.LocalName = requestDto.LocalName;
                    school.Address = requestDto.Address;
                    school.Coordinates = requestDto.Coordinates;
                    school.SchoolType = requestDto.SchoolType;
                    school.StateId = requestDto.StateId;
                    school.ZipCode = requestDto.ZipCode;
                    school.WebSite = requestDto.WebSite;
                    school.Quarter = requestDto.Quarter;
                    school.PhoneNumber = requestDto.PhoneNumber;
                    school.LocalAddress = requestDto.LocalAddress;
                    school.FaxNumber = requestDto.FaxNumber;
                    school.Facilities = requestDto.Facilities;
                    school.Email = requestDto.Email;
                    school.CityId = requestDto.CityId;
                    school.CountryId = requestDto.CountryId;
                    school.OsmId = requestDto.OsmId;

                    _ = repository.Update(school);
                }
                else
                {
                    school = new School
                    {
                        Name = requestDto.Name,
                        LocalName = requestDto.LocalName,
                        Address = requestDto.Address,
                        Coordinates = requestDto.Coordinates,
                        SchoolType = requestDto.SchoolType,
                        StateId = requestDto.StateId,
                        ZipCode = requestDto.ZipCode,
                        WebSite = requestDto.WebSite,
                        Quarter = requestDto.Quarter,
                        PhoneNumber = requestDto.PhoneNumber,
                        LocalAddress = requestDto.LocalAddress,
                        FaxNumber = requestDto.FaxNumber,
                        Facilities = requestDto.Facilities,
                        Email = requestDto.Email,
                        CityId = requestDto.CityId,
                        CountryId = requestDto.CountryId,
                        OsmId = requestDto.OsmId,
                    };
                    repository.Add(school);
                }

                _ = await uow.SaveChangesAsync();

                return new(OperationResult.Succeeded) { Data = school.Id };
            }
            catch (ReferenceConstraintException)
            {
                return new(OperationResult.NotValid) { Errors = [new() { Message = Localizer.Value["InvalidStateId"], }] };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<bool>> RemoveSchoolAsync([NotNull] ISpecification<School> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var school = await uow.GetRepository<School, int>().GetAsync(specification);
                if (school is null)
                {
                    return new(OperationResult.NotFound)
                    {
                        Data = false,
                        Errors = [new() { Message = Localizer.Value["SchoolNotFound"] },],
                    };
                }

                uow.GetRepository<School, int>().Remove(school);
                _ = await uow.SaveChangesAsync();
                return new(OperationResult.Succeeded) { Data = true };
            }
            catch (ReferenceConstraintException)
            {
                return new(OperationResult.NotValid) { Errors = [new() { Message = Localizer.Value["SchoolCantBeRemoved"], },] };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<SchoolRateDto>> GetSchoolRateAsync([NotNull] ISpecification<SchoolComment> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var result = await uow.GetRepository<SchoolComment>().GetManyQueryable(specification)
                    .GroupBy(t => t.SchoolId)
                    .Select(t => new SchoolRateDto
                    {
                        AverageRate = t.Average(c => c.AverageRate),
                        TuitionRatioRate = t.Average(c => c.TuitionRatioRate),
                        SafetyAndHappinessRate = t.Average(c => c.SafetyAndHappinessRate),
                        ITTrainingRate = t.Average(c => c.ITTrainingRate),
                        FacilitiesRate = t.Average(c => c.FacilitiesRate),
                        EducationRate = t.Average(c => c.EducationRate),
                        ClassesQualityRate = t.Average(c => c.ClassesQualityRate),
                        BehaviorRate = t.Average(c => c.BehaviorRate),
                        ArtisticActivitiesRate = t.Average(c => c.ArtisticActivitiesRate),
                        TotalCount = t.Count(),
                    }).FirstOrDefaultAsync();

                return new(OperationResult.Succeeded) { Data = result };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<ListDataSource<SchoolCommentDto>>> GetSchoolCommentsAsync(ListRequestDto<SchoolComment>? requestDto = null)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var result = await uow.GetRepository<SchoolComment>().GetManyQueryable(requestDto?.Specification).FilterListAsync(requestDto?.PagingDto);
                var users = await result.List.Select(t => new SchoolCommentDto
                {
                    Id = t.Id,
                    Comment = t.Comment,
                    CreationUser = t.CreationUser!.FullName,
                    CreationUserAvatar = t.CreationUser!.Avatar,
                    CreationDate = t.CreationDate,
                    LikeCount = t.LikeCount,
                    DislikeCount = t.DislikeCount,
                    AverageRate = t.AverageRate,
                }).ToListAsync();
                return new(OperationResult.Succeeded) { Data = new() { List = users, TotalRecordsCount = result.TotalRecordsCount } };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<long>> ManageSchoolCommentAsync([NotNull] ManageSchoolCommentRequestDto requestDto)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<SchoolComment>();
                var schoolRepository = uow.GetRepository<School, int>();
                SchoolComment? schoolComment = null;

                if (requestDto.Id.HasValue)
                {
                    schoolComment = await repository.GetAsync(requestDto.Id.Value);
                    if (schoolComment is null)
                    {
                        return new(OperationResult.NotFound)
                        {
                            Errors = [new() { Message = Localizer.Value["SchoolCommentNotFound"] },],
                        };
                    }

                    if (schoolComment.CreationUserId != requestDto.CreationUserId)
                    {
                        return new(OperationResult.NotValid)
                        {
                            Errors = [new() { Message = Localizer.Value["InvalidRequest"] },],
                        };
                    }

                    if (schoolComment.Comment != requestDto.Comment)
                    {
                        schoolComment.Status = Status.Draft;
                    }
                    schoolComment.Comment = requestDto.Comment;
                    schoolComment.CreationDate = requestDto.CreationDate;
                    schoolComment.CreationUserId = requestDto.CreationUserId;

                    schoolComment.ArtisticActivitiesRate = requestDto.ArtisticActivitiesRate;
                    schoolComment.BehaviorRate = requestDto.BehaviorRate;
                    schoolComment.ClassesQualityRate = requestDto.ClassesQualityRate;
                    schoolComment.EducationRate = requestDto.EducationRate;
                    schoolComment.FacilitiesRate = requestDto.FacilitiesRate;
                    schoolComment.ITTrainingRate = requestDto.ITTrainingRate;
                    schoolComment.SafetyAndHappinessRate = requestDto.SafetyAndHappinessRate;
                    schoolComment.TuitionRatioRate = requestDto.TuitionRatioRate;
                    schoolComment.AverageRate = Calculate(schoolComment);

                    _ = repository.Update(schoolComment);
                }
                else
                {
                    schoolComment = new SchoolComment
                    {
                        SchoolId = requestDto.SchoolId,
                        ArtisticActivitiesRate = requestDto.ArtisticActivitiesRate,
                        BehaviorRate = requestDto.BehaviorRate,
                        ClassesQualityRate = requestDto.ClassesQualityRate,
                        Comment = requestDto.Comment,
                        CreationDate = requestDto.CreationDate,
                        CreationUserId = requestDto.CreationUserId,
                        EducationRate = requestDto.EducationRate,
                        FacilitiesRate = requestDto.FacilitiesRate,
                        ITTrainingRate = requestDto.ITTrainingRate,
                        SafetyAndHappinessRate = requestDto.SafetyAndHappinessRate,
                        TuitionRatioRate = requestDto.TuitionRatioRate,
                        //Status = Status.Draft,
                        Status = Status.Confirmed,
                    };
                    schoolComment.AverageRate = Calculate(schoolComment);
                    repository.Add(schoolComment);
                }

                _ = await uow.SaveChangesAsync();
                _ = await schoolRepository.GetManyQueryable(t => t.Id == requestDto.SchoolId)
                    .ExecuteUpdateAsync(t => t.SetProperty(p => p.LastModifyDate, DateTimeOffset.UtcNow));

                return new(OperationResult.Succeeded) { Data = schoolComment.Id };

                static double Calculate(SchoolComment comment) => (
                        comment.ArtisticActivitiesRate +
                        comment.BehaviorRate +
                        comment.ClassesQualityRate +
                        comment.EducationRate +
                        comment.FacilitiesRate +
                        comment.ITTrainingRate +
                        comment.SafetyAndHappinessRate +
                        comment.TuitionRatioRate
                        ) / 8;
            }
            catch (ReferenceConstraintException)
            {
                return new(OperationResult.NotValid) { Errors = [new() { Message = Localizer.Value["InvalidData"], }] };
            }
            catch (UniqueConstraintException)
            {
                return new(OperationResult.NotValid) { Errors = [new() { Message = Localizer.Value["DuplicateData"], }] };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<bool>> LikeSchoolCommentAsync([NotNull] ISpecification<SchoolComment> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var updatedRows = await uow.GetRepository<SchoolComment>().GetManyQueryable(specification)
                    .ExecuteUpdateAsync(t => t.SetProperty(p => p.LikeCount, p => p.LikeCount + 1));

                return new(OperationResult.Succeeded) { Data = updatedRows > 0 };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<bool>> DislikeSchoolCommentAsync([NotNull] ISpecification<SchoolComment> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var updatedRows = await uow.GetRepository<SchoolComment>().GetManyQueryable(specification)
                    .ExecuteUpdateAsync(t => t.SetProperty(p => p.DislikeCount, p => p.DislikeCount + 1));

                return new(OperationResult.Succeeded) { Data = updatedRows > 0 };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<bool>> ConfirmSchoolCommentAsync([NotNull] ISpecification<SchoolComment> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var updatedRows = await uow.GetRepository<SchoolComment>().GetManyQueryable(specification)
                    .ExecuteUpdateAsync(t => t
                        .SetProperty(p => p.Status, Status.Confirmed)
                        .SetProperty(p => p.LastModifyDate, DateTimeOffset.UtcNow)
                        .SetProperty(p => p.LastModifyUserId, HttpContextAccessor.Value.HttpContext.UserId())
                    );

                return new(OperationResult.Succeeded) { Data = updatedRows > 0 };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<bool>> RejectSchoolCommentAsync([NotNull] ISpecification<SchoolComment> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var updatedRows = await uow.GetRepository<SchoolComment>().GetManyQueryable(specification)
                    .ExecuteUpdateAsync(t => t
                        .SetProperty(p => p.Status, Status.Rejected)
                        .SetProperty(p => p.LastModifyDate, DateTimeOffset.UtcNow)
                        .SetProperty(p => p.LastModifyUserId, HttpContextAccessor.Value.HttpContext.UserId())
                    );

                return new(OperationResult.Succeeded) { Data = updatedRows > 0 };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<ListDataSource<SchoolImageDto>>> GetSchoolImagesAsync(ListRequestDto<SchoolImage>? requestDto = null)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var result = await uow.GetRepository<SchoolImage>().GetManyQueryable(requestDto?.Specification).FilterListAsync(requestDto?.PagingDto);
                var users = await result.List.Select(t => new SchoolImageDto
                {
                    Id = t.Id,
                    CreationUser = t.CreationUser!.FullName,
                    CreationUserAvatar = t.CreationUser!.Avatar,
                    CreationDate = t.CreationDate,
                    FileId = t.FileId,
                    FileType = t.FileType,
                    SchoolId = t.SchoolId,
                    SchoolName = t.School!.Name,
                }).ToListAsync();
                return new(OperationResult.Succeeded) { Data = new() { List = users, TotalRecordsCount = result.TotalRecordsCount } };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<IEnumerable<string?>>> GetSchoolImagesPathAsync([NotNull] ISpecification<SchoolImage> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var result = await uow.GetRepository<SchoolImage>().GetManyQueryable(specification)
                    .Select(t => t.FileId).ToListAsync();
                return new(OperationResult.Succeeded) { Data = result.Select(t => fileService.Value.GetFileUri(t, ContainerType.School).Data?.ToString()) };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<bool>> ConfirmSchoolImageAsync([NotNull] ISpecification<SchoolImage> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var updatedRows = await uow.GetRepository<SchoolImage>().GetManyQueryable(specification)
                    .ExecuteUpdateAsync(t => t
                        .SetProperty(p => p.Status, Status.Confirmed)
                        .SetProperty(p => p.LastModifyDate, DateTimeOffset.UtcNow)
                        .SetProperty(p => p.LastModifyUserId, HttpContextAccessor.Value.HttpContext.UserId())
                    );

                return new(OperationResult.Succeeded) { Data = updatedRows > 0 };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<bool>> RejectSchoolImageAsync([NotNull] ISpecification<SchoolImage> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var updatedRows = await uow.GetRepository<SchoolImage>().GetManyQueryable(specification)
                    .ExecuteUpdateAsync(t => t
                        .SetProperty(p => p.Status, Status.Rejected)
                        .SetProperty(p => p.LastModifyDate, DateTimeOffset.UtcNow)
                        .SetProperty(p => p.LastModifyUserId, HttpContextAccessor.Value.HttpContext.UserId())
                    );

                return new(OperationResult.Succeeded) { Data = updatedRows > 0 };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }

        public async Task<ResultData<long>> CreateSchoolImageAsync([NotNull] CreateSchoolImageRequestDto requestDto)
        {
            try
            {
                using MemoryStream stream = new();
                await requestDto.File.CopyToAsync(stream);

                var fileId = await fileService.Value.UploadFileAsync(new()
                {
                    File = stream.ToArray(),
                    ContainerType = ContainerType.School,
                    FileExtension = Path.GetExtension(requestDto.File.FileName),
                });
                if (fileId.OperationResult is not OperationResult.Succeeded)
                {
                    return new(fileId.OperationResult) { Errors = fileId.Errors, };
                }

                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var imageRepository = uow.GetRepository<SchoolImage>();
                var schoolImage = new SchoolImage
                {
                    FileId = fileId.Data,
                    SchoolId = requestDto.SchoolId,
                    FileType = requestDto.FileType,
                    CreationDate = requestDto.CreationDate,
                    CreationUserId = requestDto.CreationUserId,
                    Status = Status.Draft,
                };

                var gps = ImageMetadataReader.ReadMetadata(stream)
                             .OfType<GpsDirectory>()
                             .FirstOrDefault()?.GetGeoLocation();
                if (gps is not null)
                {
                    var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
                    var point = geometryFactory.CreatePoint(new Coordinate(gps.Longitude, gps.Latitude));

                    var schoolRepository = uow.GetRepository<School, int>();
                    var schoolCoordinates = await schoolRepository.GetManyQueryable(t => t.Id == requestDto.SchoolId).Select(t => t.Coordinates).FirstOrDefaultAsync();
                    if (schoolCoordinates is not null && schoolCoordinates.Distance(point) < 2000)
                    {
                        schoolImage.Status = Status.Confirmed;
                    }
                }
                imageRepository.Add(schoolImage);
                _ = await uow.SaveChangesAsync();

                return new(OperationResult.Succeeded) { Data = schoolImage.Id };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }
    }
}
