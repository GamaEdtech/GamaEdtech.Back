namespace GamaEdtech.Backend.Shared.Service
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Backend.Common.Data;
    using GamaEdtech.Backend.Common.DataAccess.Specification;
    using GamaEdtech.Backend.Common.DataAnnotation;

    using GamaEdtech.Backend.Data.Dto.Location;
    using GamaEdtech.Backend.Data.Entity;

    [Injectable]
    public interface ILocationService
    {
        Task<ResultData<ListDataSource<LocationsDto>>> GetLocationsAsync(ListRequestDto<Location>? requestDto = null);
        Task<ResultData<LocationDto>> GetLocationAsync([NotNull] ISpecification<Location> specification);
        Task<ResultData<int>> ManageLocationAsync([NotNull] ManageLocationRequestDto requestDto);
        Task<ResultData<bool>> RemoveLocationAsync([NotNull] ISpecification<Location> specification);
    }
}
