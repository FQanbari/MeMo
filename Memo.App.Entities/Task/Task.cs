using Memo.App.Entities.Account;
using Memo.App.Entities.BaseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memo.App.Entities.Task
{
    public class Task : Entity
    {
        public Task()
        {
            IsActive = true;
            IsDelete = false;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public short Priority { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public DateTime InsertDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }

        public User User { get; set; }
        public Category Category { get; set; }
    }

    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.Priority).HasDefaultValue(1);
            builder.HasOne(p => p.Category).WithMany(p => p.Tasks).HasForeignKey(p => p.CategoryId);
            builder.HasOne(p => p.User).WithMany(p => p.Tasks).HasForeignKey(p => p.UserId);
        }
    }
}
