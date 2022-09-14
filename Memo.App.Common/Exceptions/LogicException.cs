using Memo.App.Common.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memo.App.Common.Exceptions
{
    public class LogicException : AppException
    {
        public LogicException()
            : base(Api.StatusCode.BadRequest)
        {
        }

        public LogicException(string message)
            : base(Api.StatusCode.BadRequest, message)
        {
        }

        public LogicException(object additionalData)
            : base(Api.StatusCode.BadRequest, additionalData)
        {
        }

        public LogicException(string message, object additionalData)
            : base(Api.StatusCode.BadRequest, message, additionalData)
        {
        }

        public LogicException(string message, Exception exception)
            : base(Api.StatusCode.BadRequest, message, exception)
        {
        }

        public LogicException(string message, Exception exception, object additionalData)
            : base(Api.StatusCode.BadRequest, message, exception, additionalData)
        {
        }
    }
}
