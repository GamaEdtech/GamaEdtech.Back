using Microsoft.AspNetCore.Mvc;
using GamaEdtech.Back.Application.Models;
using GamaEdtech.Back.Application.DTO.FAQManager;
using GamaEdtech.Back.Domain.Common.Utilities;
using GamaEdtech.Back.Application.Services.ApplicationServices.FAQApplicationServices;
using GamaEdtech.Back.Domain.DataAccess.Responses.FAQ;

namespace GamaEdtech.Back.Presentation.Controllers.v1
{
    [ApiVersion("1")]
    public class FAQController(IFAQManager fAQManager) : BaseController
    {
        private readonly IFAQManager _fAQManager = fAQManager;

        [HttpGet("[action]")]
        public virtual async Task<ActionResult<List<FAQResponse>>> GetFAQsWithDynamicFilter([FromQuery] GetFAQWithDynamicFilterDTO getFAQWithDynamicFilterDTO, CancellationToken cancellationToken)
        {
            var result = await _fAQManager.GetFAQWithDynamicFilter(getFAQWithDynamicFilterDTO, cancellationToken);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public virtual async Task<ActionResult> CreateForum([FromForm] CreateForumDTO createForumDTO, CancellationToken cancellationToken)
        {
            await _fAQManager.AddForum(createForumDTO, cancellationToken);
            return Ok();
        }

        [HttpPost("[action]")]
        public virtual async Task<ActionResult> CreateFAQCategory(CreateFAQCategoryDTO createFAQCategoryDTO, CancellationToken cancellationToken)
        {
            await _fAQManager.CreateFAQCategory(createFAQCategoryDTO, cancellationToken);
            return Ok();
        }

        [HttpGet("[action]")]
        public virtual async Task<ActionResult<List<FAQCategoryResponse>>> GetFAQCategoryHierarchy([FromQuery] CustomDateFormat customDateFormat = CustomDateFormat.ToSolarDate, CancellationToken cancellationToken = default)
        {
            var result = await _fAQManager.GetFAQCategoryHierarchy(customDateFormat, cancellationToken);
            return Ok(result);
        }
    }
}