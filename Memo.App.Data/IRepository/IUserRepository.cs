using Data.Repositories;
using Memo.App.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memo.App.Data.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User> FindUserByUserNameAsync(string userName, CancellationToken cancellationToken);
        public Task<bool> CheckUserNameAndPasswordAsync(string userName,string password, CancellationToken cancellationToken);
        Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
        Task UpdateSecurityStapAsync(User user, CancellationToken cancellationToken);
        void UpdateSecurityStap(User user, bool saveNow = true);
    }
}
