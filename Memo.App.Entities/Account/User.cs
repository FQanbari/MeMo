using Memo.App.Entities.BaseEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memo.App.Entities.Account
{
    public class User: IdentityUser<int>,IEntity
    {
        public User()
        {
            IsActive = true;
            IsDelete = false;
            InsertDate = DateTime.Now;
        }
        public string Name { get; set; }
        public string Family { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime LastLoginDate { get; set; }

        public ICollection<Memo.App.Entities.Task.Task> Tasks { get; set; }
    }
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Family).IsRequired().HasMaxLength(50);
            builder.Property(p => p.UserName).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Password).IsRequired().HasMaxLength(100);
        }
    }
}
