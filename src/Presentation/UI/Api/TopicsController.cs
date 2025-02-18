namespace GamaEdtech.UI.Web.Api
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;

    using GamaEdtech.Data.Entity;
    using GamaEdtech.Data.Specification.Topic;
    using GamaEdtech.Data.ViewModel.Topic;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TopicsController(Lazy<ILogger<TopicsController>> logger, Lazy<ITopicService> boardService)
        : ApiControllerBase<TopicsController>(logger)
    {
        [HttpGet, Produces<ApiResponse<ListDataSource<TopicsResponseViewModel>>>()]
        public async Task<IActionResult> GetTopics([NotNull, FromQuery] TopicsRequestViewModel request)
        {
            try
            {
                var result = await boardService.Value.GetTopicsAsync(new ListRequestDto<Topic>
                {
                    PagingDto = request.PagingDto,
                    Specification = request.SubjectId.HasValue ? new SubjectIdEqualsSpecification(request.SubjectId.Value) : null,
                });
                return Ok(new ApiResponse<ListDataSource<TopicsResponseViewModel>>
                {
                    Errors = result.Errors,
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new TopicsResponseViewModel
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Order = t.Order,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ListDataSource<TopicsResponseViewModel>> { Errors = [new() { Message = exc.Message }] });
            }
        }
    }
}
