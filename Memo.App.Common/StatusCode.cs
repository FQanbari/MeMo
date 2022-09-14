namespace Memo.App.Common.Api
{
    public enum StatusCode
    {
        Unknown = 0,
        Success = 1,
        Exception = -1,
        DataNotFound = -2,
        BadRequest = -4,
        LogicError = 5,
        ServerError = 6,
        UnAuthorized = 7
    }
}
