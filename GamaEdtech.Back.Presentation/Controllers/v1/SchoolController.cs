using GamaEdtech.Back.Application.DTO.SchoolManager;
using GamaEdtech.Back.Application.Models;
using GamaEdtech.Back.Application.Services.ApplicationServices.SchoolApplicationServices;
using Microsoft.AspNetCore.Mvc;

namespace GamaEdtech.Back.Presentation.Controllers.v1
{
    [ApiVersion("1")]
    public class SchoolController(ISchoolManager schoolManager) : BaseController
    {

        [HttpPost("[action]")]
        public virtual async Task<ActionResult> CreateSchool(CreateSchoolDTO createSchoolDTO, CancellationToken cancellationToken)
        {
            await schoolManager.CreateSchool(createSchoolDTO, cancellationToken);
            return Ok(schoolManager);
        }
    }
}