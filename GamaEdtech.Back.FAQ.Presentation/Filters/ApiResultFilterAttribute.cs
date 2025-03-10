using GamaEdtech.Back.FAQ.Application.Models;
using GamaEdtech.Back.FAQ.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GamaEdtech.Back.FAQ.Application.Filters
{
    public class ApiResultFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult1)
            {
                var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, objectResult1.Value);
                context.Result = new JsonResult(apiResult.Data);
            }
            if (context.Result is ObjectResult objectResult && context.ModelState.IsValid)
            {
                var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, objectResult.Value);
                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is OkResult okResult)
            {
                var apiResult = new ApiResult(true, ApiResultStatusCode.Success);
                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is BadRequestResult badRequestResult)
            {
                var apiResult = new ApiResult(false, ApiResultStatusCode.BadRequest);
                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is BadRequestObjectResult badRequestObjectResult)
            {
                string message = badRequestObjectResult.Value.ToString();
                if (badRequestObjectResult.Value is SerializableError error)
                {
                    IEnumerable<string> errorMessage = error.SelectMany(p => (string[])p.Value).Distinct();
                    message = string.Join("|", errorMessage);
                }
                var apiResult = new ApiResult<object>(false, ApiResultStatusCode.BadRequest, badRequestObjectResult.Value);
                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is ContentResult contentResult)
            {
                var apiResult = new ApiResult(true, ApiResultStatusCode.Success, contentResult.Content);
                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is NotFoundResult notFoundResult)
            {
                var apiResult = new ApiResult(false, ApiResultStatusCode.NotFound);
                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is NotFoundObjectResult notFoundObjectResult)
            {
                var apiResult = new ApiResult<object>(false, ApiResultStatusCode.NotFound, notFoundObjectResult.Value);
                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is ObjectResult ObjectResult && ObjectResult.StatusCode == null
                && !(ObjectResult.Value is ApiResult))
            {
                var apiResult = new ApiResult<object>(true, ApiResultStatusCode.NotFound, ObjectResult.Value);
                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is ChallengeResult challengeResult)
            {
                var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, challengeResult);
                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is BadRequestObjectResult)
            {

            }
            base.OnResultExecuting(context);
        }
    }

}
