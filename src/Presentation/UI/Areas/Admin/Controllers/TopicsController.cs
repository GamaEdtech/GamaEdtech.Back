namespace GamaEdtech.Backend.UI.Web.Areas.Admin.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Backend.Common.Core;
    using GamaEdtech.Backend.Common.Data;
    using GamaEdtech.Backend.Common.DataAccess.Specification.Impl;

    using GamaEdtech.Backend.Data.Dto.Topic;
    using GamaEdtech.Backend.Data.Entity;
    using GamaEdtech.Backend.Data.Specification.Topic;
    using GamaEdtech.Backend.Data.ViewModel.Topic;
    using GamaEdtech.Backend.Shared.Service;

    using Microsoft.AspNetCore.Mvc;

    [Common.DataAnnotation.Area(nameof(Admin), "Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ApiVersion("1.0")]
    //[Permission(Roles = [nameof(Role.Admin)])]
    public class TopicsController(Lazy<ILogger<TopicsController>> logger, Lazy<ITopicService> topicService)
        : ApiControllerBase<TopicsController>(logger)
    {
        [HttpGet, Produces<ApiResponse<ListDataSource<TopicsResponseViewModel>>>()]
        public async Task<IActionResult> GetTopics([NotNull, FromQuery] TopicsRequestViewModel request)
        {
            try
            {
                var result = await topicService.Value.GetTopicsAsync(new ListRequestDto<Topic>
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

        [HttpGet("{id:int}"), Produces<ApiResponse<TopicResponseViewModel>>()]
        public async Task<IActionResult> GetTopic([FromRoute] int id)
        {
            try
            {
                var result = await topicService.Value.GetTopicAsync(new IdEqualsSpecification<Topic, int>(id));
                return Ok(new ApiResponse<TopicResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = result.Data is null ? null : new()
                    {
                        Id = result.Data.Id,
                        Title = result.Data.Title,
                        Order = result.Data.Order,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<TopicResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPost, Produces<ApiResponse<ManageTopicResponseViewModel>>()]
        public async Task<IActionResult> CreateTopic([NotNull] ManageTopicRequestViewModel request)
        {
            try
            {
                var result = await topicService.Value.ManageTopicAsync(new ManageTopicRequestDto
                {
                    Title = request.Title,
                    Order = request.Order,
                });
                return Ok(new ApiResponse<ManageTopicResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = new() { Id = result.Data, },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ManageTopicResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPut("{id:int}"), Produces<ApiResponse<ManageTopicResponseViewModel>>()]
        public async Task<IActionResult> UpdateTopic([FromRoute] int id, [NotNull, FromBody] ManageTopicRequestViewModel request)
        {
            try
            {
                var result = await topicService.Value.ManageTopicAsync(new ManageTopicRequestDto
                {
                    Id = id,
                    Title = request.Title,
                    Order = request.Order,
                });
                return Ok(new ApiResponse<ManageTopicResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = new() { Id = result.Data, },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ManageTopicResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpDelete("{id:int}"), Produces<ApiResponse<bool>>()]
        public async Task<IActionResult> RemoveTopic([FromRoute] int id)
        {
            try
            {
                var result = await topicService.Value.RemoveTopicAsync(new IdEqualsSpecification<Topic, int>(id));
                return Ok(new ApiResponse<bool>
                {
                    Errors = result.Errors,
                    Data = result.Data
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<bool> { Errors = [new() { Message = exc.Message }] });
            }
        }
    }
}
