using Microsoft.AspNetCore.Mvc;
using GamaEdtech.Back.FAQ.Application.Models;
using GamaEdtech.Back.FAQ.Application.DTO.FAQManager;
using GamaEdtech.Back.FAQ.Application.Services.ApplicationServices.FAQApplicationServices;
using GamaEdtech.Back.FAQ.Domain.Common.Utilities;
using GamaEdtech.Back.FAQ.Domain.DataAccess.Mapper.FAQ;

namespace GamaEdtech.Back.FAQ.Application.Controllers.v1
{
    [ApiVersion("1")]
    public class FAQController(IFAQManager fAQManager) : BaseController
    {
        private readonly IFAQManager _fAQManager = fAQManager;

        [HttpGet("[action]")]
        public virtual async Task<ActionResult<List<FAQResult>>> GetFAQsWithDynamicFilter([FromQuery] GetFAQWithDynamicFilterDTO getFAQWithDynamicFilterDTO, CancellationToken cancellationToken)
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
        public virtual async Task<ActionResult<List<FAQCategoryResult>>> GetFAQCategoryHierarchy([FromQuery] CustomDateFormat customDateFormat = CustomDateFormat.ToSolarDate, CancellationToken cancellationToken = default)
        {
            var result = await _fAQManager.GetFAQCategoryHierarchy(customDateFormat, cancellationToken);
            return Ok(result);
        }
    }
}