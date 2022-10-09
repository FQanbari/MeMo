using Memo.App.Common;
using Memo.App.Data;
using Memo.App.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memo.App.WebFramework.Configuration
{
    public static class IdenitityExtensions
    {
        public static void AddCustomIdentity(this IServiceCollection services, IdentitySettings settings)
        {
            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = settings.PasswordRequireDigit;
                options.Password.RequiredLength = settings.PasswordRequiredLength;
                options.Password.RequireNonAlphanumeric = settings.PasswordRequireNonAlphanumeric;
                options.Password.RequireUppercase = settings.PasswordRequireUppercase;
                options.Password.RequireLowercase = settings.PasswordRequireLowercase;

                options.User.RequireUniqueEmail = settings.UserRequireUniqueEmail;
            })
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();
        }
    }
}
