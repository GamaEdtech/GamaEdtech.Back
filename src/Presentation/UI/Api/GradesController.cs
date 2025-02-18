namespace GamaEdtech.UI.Web.Api
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;

    using GamaEdtech.Data.Entity;
    using GamaEdtech.Data.Specification.Grade;
    using GamaEdtech.Data.ViewModel.Grade;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class GradesController(Lazy<ILogger<GradesController>> logger, Lazy<IGradeService> boardService)
        : ApiControllerBase<GradesController>(logger)
    {
        [HttpGet, Produces<ApiResponse<ListDataSource<GradesResponseViewModel>>>()]
        public async Task<IActionResult> GetGrades([NotNull, FromQuery] GradesRequestViewModel request)
        {
            try
            {
                var result = await boardService.Value.GetGradesAsync(new ListRequestDto<Grade>
                {
                    PagingDto = request.PagingDto,
                    Specification = request.BoardId.HasValue ? new BoardIdEqualsSpecification(request.BoardId.Value) : null,
                });
                return Ok(new ApiResponse<ListDataSource<GradesResponseViewModel>>
                {
                    Errors = result.Errors,
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new GradesResponseViewModel
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Icon = t.Icon,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ListDataSource<GradesResponseViewModel>> { Errors = [new() { Message = exc.Message }] });
            }
        }
    }
}
