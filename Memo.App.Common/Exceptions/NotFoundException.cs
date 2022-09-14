using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memo.App.Common.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException()
            : base(Api.StatusCode.BadRequest)
        {
        }

        public NotFoundException(string message)
            : base(Api.StatusCode.BadRequest, message)
        {
        }

        public NotFoundException(object additionalData)
            : base(Api.StatusCode.BadRequest, additionalData)
        {
        }

        public NotFoundException(string message, object additionalData)
            : base(Api.StatusCode.BadRequest, message, additionalData)
        {
        }

        public NotFoundException(string message, Exception exception)
            : base(Api.StatusCode.BadRequest, message, exception)
        {
        }

        public NotFoundException(string message, Exception exception, object additionalData)
            : base(Api.StatusCode.BadRequest, message, exception, additionalData)
        {
        }
    }
}
