using Memo.App.WebFramework.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memo.App.WebFramework.Filter
{
    public class ApiResultAtribute: ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if(context.Result is OkObjectResult okObjectResult)
            {
                var apiResult = new ApiResult<object>(true, StatusCode.Success, okObjectResult.Value);
                context.Result = new JsonResult(apiResult){ StatusCode = okObjectResult.StatusCode};
            }
            else if(context.Result is OkResult okResult)
            {
                var apiResult = new ApiResult(true, StatusCode.Success);
                context.Result = new JsonResult(apiResult) { StatusCode = okResult.StatusCode };
            }
            else if(context.Result is BadRequestResult badRequestResult)
            {
                var apiResult = new ApiResult(false, StatusCode.BadRequest);
                context.Result = new JsonResult(apiResult) { StatusCode = badRequestResult.StatusCode };
            }
            else if(context.Result is BadRequestObjectResult badRequestObjectResult)
            {
                var message = badRequestObjectResult.Value.ToString();
                if(badRequestObjectResult.Value is SerializableError errors)
                {
                    var errorsMessage = errors.SelectMany(p => (string[])p.Value).Distinct();
                    message = string.Join(" | ", errorsMessage);
                }
                var apiResult = new ApiResult(false, StatusCode.BadRequest, message);
                context.Result = new JsonResult(apiResult) { StatusCode = badRequestObjectResult.StatusCode };
            }
            else if(context.Result is ContentResult contentResult)
            {
                var apiResult = new ApiResult(true, StatusCode.Success, contentResult.Content);
                context.Result = new JsonResult(apiResult) { StatusCode = contentResult.StatusCode };
            }
            else if(context.Result is NotFoundResult notFoundResult)
            {
                var apiResult = new ApiResult(false, StatusCode.DataNotFound);
                context.Result = new JsonResult(apiResult) { StatusCode=notFoundResult.StatusCode };
            }
            else if(context.Result is NotFoundObjectResult notFoundObjectResult)
            {
                var apiResult = new ApiResult<object>(false, StatusCode.DataNotFound, notFoundObjectResult.Value);
                context.Result = new JsonResult(apiResult) { StatusCode = notFoundObjectResult.StatusCode };
            }
            else if(context.Result is ObjectResult objectResult && objectResult.StatusCode == null && !(objectResult.Value is ApiResult))
            {
                var apiResult = new ApiResult<object>(true, StatusCode.Success, objectResult.Value);
                context.Result = new JsonResult(apiResult) { StatusCode = objectResult.StatusCode };
            }
            base.OnResultExecuting(context);
        }
    }
}
