using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memo.App.Common.Exceptions
{
    public class BadRequestException : AppException
    {
        public BadRequestException()
           : base(Api.StatusCode.BadRequest)
        {
        }

        public BadRequestException(string message)
            : base(Api.StatusCode.BadRequest, message)
        {
        }

        public BadRequestException(object additionalData)
            : base(Api.StatusCode.BadRequest, additionalData)
        {
        }

        public BadRequestException(string message, object additionalData)
            : base(Api.StatusCode.BadRequest, message, additionalData)
        {
        }

        public BadRequestException(string message, Exception exception)
            : base(Api.StatusCode.BadRequest, message, exception)
        {
        }

        public BadRequestException(string message, Exception exception, object additionalData)
            : base(Api.StatusCode.BadRequest, message, exception, additionalData)
        {
        }
    }
}
