namespace Memo.App.Api.Helper
{
    public class LoggingHelper
    {
        //I wish to log in places outside the controller
        //for example here.
        public class GenericHelper
        {
            private readonly ILogger<GenericHelper> _logger;
            public GenericHelper(ILogger<GenericHelper> logger)
            {
                _logger = logger;
                _logger.LogInformation(1, "GenericHelper has been constructed");
            }
            public void JustADumbFunctionCall()
            {
                _logger.LogInformation("JustADumbFunctionCall has been called");
            }
        }
    }
}
