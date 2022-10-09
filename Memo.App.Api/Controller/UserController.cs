using Common.Utilities;
using Memo.App.Data.IRepository;
using Memo.App.Entities.Account;
using Memo.App.WebFramework.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Memo.App.WebFramework.Filter;
using Memo.App.Api.Models;
using Memo.App.Common.Exceptions;
using Memo.App.Services.Idenitity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Memo.App.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultAtribute]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IJwtService jwtService;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public UserController(IUserRepository userRepository, IJwtService jwtService, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.userRepository = userRepository;
            this.jwtService = jwtService;
            this.userManager = userManager;
            this.roleManager = roleManager;
            //this.jwtService = jwtService;
        }
        // GET: api/<UserController>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<User>>> Get(CancellationToken cancellationToken)
        {
            var users = await userRepository.TableNoTracking.ToListAsync(cancellationToken);
            return users;
        }

        // GET api/<UserController>/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<User> Get(int id, CancellationToken cancellationToken)
        {
            var user2 = await userManager.FindByIdAsync(id.ToString());
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
                throw new NotFoundException("user not found!");

            return user;
        }
        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<string> Token(string username,string password,CancellationToken cancellationToken)
        {
            //var user = await userRepository.FindUserByUserNameAsync(username, cancellationToken);
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
                throw new BadRequestException("این کاربر وجود ندارد");

            //var exist = await userRepository.CheckUserNameAndPasswordAsync(user.UserName, user.Password, cancellationToken);
            var checkPass = await userManager.CheckPasswordAsync(user, password);
            if (!checkPass)
                throw new BadRequestException("رمز وارد شده اشتباه است");

            var jwt = await jwtService.GenerateAsync(user);
            return jwt;
        }
        // POST api/<UserController>
        [HttpPost]
        public async Task<ApiResult<UserDto>> Post(UserDto model, CancellationToken cancellationToken)
        {
            //var exisit = userRepository.TableNoTracking.Any(p => p.UserName == model.UserName);
            //if (exisit)
            //    throw new BadRequestException("کاربر وجود ندارد.");
            var user = new User();
            user.Name = model.Name;
            user.UserName = model.UserName;
            user.Password = SecurityHelper.GetSha256Hash(model.Password);
            user.Family = model.Family;
            user.Age = model.Age;
            user.Email = model.Email;

            //await userRepository.AddAsync(user, cancellationToken);
            //var list = new List<string>();
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var isExistRole = await roleManager.FindByNameAsync("Guest");
                if (isExistRole == null)
                {
                    var result2 = await roleManager.CreateAsync(new Role { Name = "Admin" });
                    if (!result2.Succeeded)
                        throw new BadRequestException("role not created.");
                }

                var result3 = await userManager.AddToRoleAsync(user, "Guest");
                if (!result3.Succeeded)
                    throw new BadRequestException("Role not added in user");

            }
            else
            {
                string errorMessage = ""; ;
                foreach(var error in result.Errors)
                {
                    errorMessage += error.Code + " " + error.Description;
                }
                throw new BadRequestException(errorMessage);
            }
               
            return Ok(model);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id:int}")]
        [AllowAnonymous]
        public async Task<ApiResult> Put(int id, UserDto user, CancellationToken cancellationToken)
        {
            //var updateUser = await userRepository.GetByIdAsync(cancellationToken, id);
            var updateUser = await userManager.FindByIdAsync(id.ToString());
            updateUser.Name = user.Name;
            updateUser.UserName = user.UserName;
            updateUser.Family = user.Family;
            updateUser.Age = user.Age;

            var identityResult = await userManager.UpdateSecurityStampAsync(updateUser);
            if (!identityResult.Succeeded)
                throw new BadRequestException("SecurityStamp can not update.");

            await userRepository.UpdateAsync(updateUser, cancellationToken);
            return Ok();
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            await userRepository.DeleteAsync(user, cancellationToken);
            return Ok();
        }
    }
}
