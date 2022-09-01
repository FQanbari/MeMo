using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memo.App.WebFramework.Api
{
    public class ApiResult
    {
        public bool IsSuccess { get; set; }    
        public StatusCode StatusCode { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
        public ApiResult(bool isSuccess, StatusCode statusCode, string message = null)
        {
            IsSuccess = isSuccess;
            statusCode = statusCode;
            Message = message ?? statusCode.ToString();
        }
        #region Implicit Operator
        public static implicit operator ApiResult(OkResult result)
        {
            return new ApiResult(true, StatusCode.Success);
        }
        public static implicit operator ApiResult(BadRequestResult result)
        {
            return new ApiResult(false, StatusCode.BadRequest);
        }
        public static implicit operator ApiResult(BadRequestObjectResult result)
        {
            var pi = result.Value.GetType().GetProperty("Errors");
            var errors = (Dictionary<string, string[]>)(pi.GetValue(result.Value, null));

            var message = result.ToString();
            var errorMessage = errors.SelectMany(p => p.Value).Distinct();
            message = string.Join(" | ", errorMessage);
            return new ApiResult(false, StatusCode.BadRequest, message);
        }
        public static implicit operator ApiResult(ContentResult result)
        {
            return new ApiResult(true, StatusCode.Success, result.Content);
        }
        public static implicit operator ApiResult(NotFoundResult result)
        {
            return new ApiResult(false, StatusCode.DataNotFound);
        }
        #endregion
    }
    public class ApiResult<TData> : ApiResult
        where TData : class
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TData Data { get; set; }
        public ApiResult(bool isSuccess, StatusCode statusCode,TData data, string message = null) 
            : base(isSuccess, statusCode, message)
        {
            Data = data;
        }

        #region Implicit Operator
        public static implicit operator ApiResult<TData>(TData data)
        {
            return new ApiResult<TData>(true, StatusCode.Success, data);
        }
        public static implicit operator ApiResult<TData>(OkResult result)
        {
            return new ApiResult<TData>(true, StatusCode.Success, null);
        }
        public static implicit operator ApiResult<TData>(OkObjectResult result)
        {
            return new ApiResult<TData>(true, StatusCode.Success, (TData)result.Value);
        }
        public static implicit operator ApiResult<TData>(BadRequestResult result)
        {
            return new ApiResult<TData>(false, StatusCode.BadRequest, null);
        }
        public static implicit operator ApiResult<TData>(BadRequestObjectResult result)
        {
            var pi = result.Value.GetType().GetProperty("Errors");
            var errors = (Dictionary<string, string[]>)(pi.GetValue(result.Value, null));

            var message = result.ToString();
            var errorMessage = errors.SelectMany(p => p.Value).Distinct();
            message = string.Join(" | ", errorMessage);
            return new ApiResult<TData>(false, StatusCode.BadRequest,null, message);
        }
        public static implicit operator ApiResult<TData>(ContentResult result)
        {
            return new ApiResult<TData>(true, StatusCode.Success,null, result.Content);
        }
        public static implicit operator ApiResult<TData>(NotFoundResult result)
        {
            return new ApiResult<TData>(false, StatusCode.DataNotFound, null);
        }
        public static implicit operator ApiResult<TData>(NotFoundObjectResult result)
        {
            return new ApiResult<TData>(false, StatusCode.DataNotFound, (TData)result.Value);
        }
        #endregion
    }
}
