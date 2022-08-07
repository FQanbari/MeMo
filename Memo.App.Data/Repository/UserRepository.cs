using Data.Repositories;
using Memo.App.Data.IRepository;
using Memo.App.Entities.Account;
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
    }
}
