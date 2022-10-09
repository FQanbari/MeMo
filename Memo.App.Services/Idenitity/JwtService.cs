using Memo.App.Common;
using Memo.App.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Memo.App.Services.Idenitity
{
    public class JwtService : IScopedDependency, IJwtService
    {
        private readonly SiteSettings _siteSettings;
        private readonly SignInManager<User> signInManager;

        public JwtService(IOptionsSnapshot<SiteSettings> settings, SignInManager<User> signInManager)
        {
            _siteSettings = settings.Value;
            this.signInManager = signInManager;
        }
        public async Task<string> GenerateAsync(User user)
        {
            var securityKey = Encoding.UTF8.GetBytes(_siteSettings.JwtSettings.SecretKey);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionKey = Encoding.UTF8.GetBytes(_siteSettings.JwtSettings.EncryptKey);
            var ecryptionCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionKey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            var claims = await _getClaimsAsync(user);
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _siteSettings.JwtSettings.Issuer,
                IssuedAt = DateTime.Now,
                Audience = _siteSettings.JwtSettings.Audience,
                Expires = DateTime.Now.AddDays(_siteSettings.JwtSettings.ExpirationDays),
                NotBefore = DateTime.Now,
                SigningCredentials = signingCredentials,
                Subject = new ClaimsIdentity(claims),
                EncryptingCredentials = ecryptionCredentials
            };

            var tokenHanler = new JwtSecurityTokenHandler();
            var securityToken = tokenHanler.CreateToken(descriptor);
            var jwt = tokenHanler.WriteToken(securityToken);

            return jwt;
        }

        private async Task<IEnumerable<Claim>> _getClaimsAsync(User user)
        {
            var claimsPrincipal = await signInManager.ClaimsFactory.CreateAsync(user);

            return claimsPrincipal.Claims;
            //var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;
            //var list = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, user.Name),
            //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //    new Claim(securityStampClaimType, user.SecurityStamp.ToString())
            //};

            //var roles = new Role[] { new Role { Name = "geust" } };

            //foreach (var role in roles)
            //    list.Add(new Claim(ClaimTypes.Role, role.Name));

            //return list;
        }
    }
}
