using Memo.App.Common.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Memo.App.Common.Exceptions
{
    public class AppException:Exception
    {
        public StatusCode StatusCode { get; set; }
        public HttpStatusCode HttpStatusCode{ get; set; }
        public object AdditionalData{ get; set; }

        public AppException()
            :this(StatusCode.ServerError)
        {
        }
        public AppException(StatusCode statusCode)
            :this(statusCode, null)
        {
        }
        public AppException(string message)
            :this(StatusCode.ServerError,message)
        {
        }
        public AppException(StatusCode statusCode, string message)
            :this(statusCode,message,HttpStatusCode.InternalServerError)
        {
        }
        public AppException(string message,object additionalData)
            :this(StatusCode.ServerError,message,additionalData)
        {
        }
        public AppException(StatusCode statusCode, object additionalData)
            :this(statusCode,null,additionalData)
        {
        }
        public AppException(StatusCode statusCode,string message,object additionalData)
            :this(statusCode,message,HttpStatusCode.InternalServerError,additionalData)
        {
        }
        public AppException(StatusCode statusCode,string message, HttpStatusCode httpStatusCode)
            :this(statusCode,message,httpStatusCode,null)
        {
        }
        public AppException(StatusCode statusCode,string message,HttpStatusCode httpStatusCode,object additionalData)
            :this(statusCode,message,httpStatusCode,null,additionalData)
        {
        }
        public AppException(string message, Exception exception)
            : this(StatusCode.ServerError, message, exception)
        {
        }
        public AppException(string message,Exception exception,object additionalData)
            :this(StatusCode.ServerError,message,exception,additionalData)
        {
        }
        public AppException(StatusCode statusCode,string message,Exception exception)
            :this(statusCode,message,HttpStatusCode.InternalServerError,exception)
        {
        }
        public AppException(StatusCode statusCode,string message,Exception exception, object additionalData)
            :this(statusCode,message,HttpStatusCode.InternalServerError,exception,additionalData)
        {
        }
        public AppException(StatusCode statusCode, string message, HttpStatusCode httpStatusCode, Exception exception)
            :this(statusCode,message,httpStatusCode,exception,null)
        {
        }
        
        public AppException(StatusCode statusCode,string message,HttpStatusCode httpStatusCode,Exception exception,object additionalData)
            :base(message,exception)
        {
            StatusCode = statusCode;
            HttpStatusCode = httpStatusCode;
            AdditionalData = additionalData;
        }             
    }
}
