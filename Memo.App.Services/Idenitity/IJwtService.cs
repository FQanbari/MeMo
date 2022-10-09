using Memo.App.Entities.Account;

namespace Memo.App.Services.Idenitity
{
    public interface IJwtService
    {
        Task<string> GenerateAsync(User user);
    }
}