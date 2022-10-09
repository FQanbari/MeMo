using Data.Repositories;
using Memo.App.Common;
using Memo.App.Common.Exceptions;
using Memo.App.Data.IRepository;
using Memo.App.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memo.App.Data.Repository
{
    public class UserRepository : Repository<User>,IScopedDependency, IUserRepository
    {
        private readonly UserManager<User> userManager;

        public UserRepository(ApplicationDbContext dbContext, UserManager<User> userManager)
            : base(dbContext)
        {
            this.userManager = userManager;
        }

        public Task<bool> CheckUserNameAndPasswordAsync(string userName, string password, CancellationToken cancellationToken)
        {
            return Table.AnyAsync(p => p.UserName == userName && p.Password == password,cancellationToken);
        }

        public Task<User> FindUserByUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            return Table.Where(p => p.UserName == userName).FirstOrDefaultAsync(cancellationToken);
        }

        public Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
        {
            user.LastLoginDate = DateTime.Now;
            return UpdateAsync(user, cancellationToken);
        }

        public override async Task UpdateAsync(User entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            //entity.SecurityStamp = Guid.NewGuid();           
            await base.UpdateAsync(entity, cancellationToken, saveNow);
        }

        public override void Update(User entity, bool saveNow = true)
        {
            //entity.SecurityStamp = Guid.NewGuid();
            base.Update(entity, saveNow);
        }

        public override void UpdateRange(IEnumerable<User> entities, bool saveNow = true)
        {
            //foreach (var entity in entities)
            //    entity.SecurityStamp = Guid.NewGuid();

            base.UpdateRange(entities, saveNow);
        }

        public override Task UpdateRangeAsync(IEnumerable<User> entities, CancellationToken cancellationToken, bool saveNow = true)
        {
            //foreach (var entity in entities)
            //    entity.SecurityStamp = Guid.NewGuid();

            return base.UpdateRangeAsync(entities, cancellationToken, saveNow);
        }

        public Task UpdateSecurityStapAsync(User user, CancellationToken cancellationToken)
        {
            //user.SecurityStamp = Guid.NewGuid();
            return UpdateAsync(user, cancellationToken);
        }

        public void UpdateSecurityStap(User user, bool saveNow = true)
        {
            //user.SecurityStamp = Guid.NewGuid();
            Update(user, saveNow);
        }
    }
}
