using Data.Repositories;
using Memo.App.Data.IRepository;
using Memo.App.Entities.Account;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memo.App.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) 
            : base(dbContext)
        {
        }

        public Task<bool> CheckUserNameAndPassword(string userName, string password, CancellationToken cancellationToken)
        {
            return Table.AnyAsync(p => p.UserName == userName && p.Password == password,cancellationToken);
        }

        public Task<User> FindUserByUserName(string userName, CancellationToken cancellationToken)
        {
            return Table.Where(p => p.UserName == userName).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
