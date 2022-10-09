using Memo.App.Common.Api;
using Memo.App.Common.Exceptions;
using Memo.App.WebFramework.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Memo.App.WebFramework.Middleware
{
    public static class UseCustomExceptionHandlerMiddleware
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomMiddlewareHandler>();
        }
    }
    public class CustomMiddlewareHandler
    {
        private readonly RequestDelegate _next;
        private readonly IHostingEnvironment _env;
        private readonly ILogger<CustomMiddlewareHandler> _logger;

        public CustomMiddlewareHandler(RequestDelegate next, IHostingEnvironment env, ILogger<CustomMiddlewareHandler> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            string message = null;
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            StatusCode statusCode = StatusCode.ServerError;
            try
            {
                await _next(httpContext);
            }
            catch(AppException ex)
            {
                _logger.LogError(ex, ex.Message);
                httpStatusCode = ex.HttpStatusCode;
                statusCode = ex.StatusCode;

                if (_env.IsDevelopment())
                {
                    var dic = new Dictionary<string, string>
                    {
                        ["Exception"] = ex.Message,
                        ["StackTrace"] = ex.StackTrace
                    };
                    if (ex.InnerException != null)
                    {
                        dic.Add("InnerException.Exception", ex.InnerException.Message);
                        dic.Add("InnerException.StackTrace", ex.InnerException.StackTrace);
                    }
                    if (ex.AdditionalData != null)
                        dic.Add("AdditionalData", JsonConvert.SerializeObject(ex.AdditionalData));

                    message = JsonConvert.SerializeObject(dic);
                }
                else
                {
                    message = ex.Message;
                }
                await WriteToResponseAsync();
            }
            catch (SecurityTokenExpiredException ex)
            {
                _logger.LogError(ex, ex.Message);
                SetUnAuthorizeResponse(ex);
                await WriteToResponseAsync();
            }
            catch(UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, ex.Message);
                SetUnAuthorizeResponse(ex);
                await WriteToResponseAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                if (_env.IsDevelopment())
                {
                    var dic = new Dictionary<string, string>
                    {
                        ["Exception"] = ex.Message,
                        ["StackTrace"] = ex.StackTrace
                    };

                    message = JsonConvert.SerializeObject(dic);
                }
                await WriteToResponseAsync();
            }
           async Task WriteToResponseAsync()
            {
                if (httpContext.Response.HasStarted)
                    throw new InvalidOperationException("The response has already started, the http status code middleware will not be executed.");

                var apiResult = new ApiResult(false, statusCode, message);
                var json = JsonConvert.SerializeObject(apiResult);

                httpContext.Response.StatusCode = (int)httpStatusCode;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(json);
            }

            void SetUnAuthorizeResponse(Exception ex)
            {
                statusCode = StatusCode.UnAuthorized;
                httpStatusCode = HttpStatusCode.Unauthorized;

                if (_env.IsDevelopment())
                {
                    var dic = new Dictionary<string, string>
                    {
                        ["Exception"] = ex.Message,
                        ["StackTrace"] = ex.StackTrace
                    };

                    if (ex is SecurityTokenExpiredException tokenException)
                        dic.Add("Expires", tokenException.Expires.ToString());

                    message = JsonConvert.SerializeObject(dic);
                }
            }
        }
    }
}
              
