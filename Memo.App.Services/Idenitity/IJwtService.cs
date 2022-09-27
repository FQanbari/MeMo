using Memo.App.Entities.Account;

namespace Memo.App.Services.Idenitity
{
    public interface IJwtService
    {
        string Generate(User user);
    }
}