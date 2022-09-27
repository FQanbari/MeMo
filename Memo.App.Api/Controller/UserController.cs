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

        public UserController(IUserRepository userRepository, IJwtService jwtService)
        {
            this.userRepository = userRepository;
            this.jwtService = jwtService;
            //this.jwtService = jwtService;
        }
        // GET: api/<UserController>
        [HttpGet]
        [Authorize(Roles = "geust")]
        public async Task<ApiResult<List<User>>> Get(CancellationToken cancellationToken)
        {
            var users = await userRepository.TableNoTracking.ToListAsync(cancellationToken);
            return users;
        }

        // GET api/<UserController>/5
        [HttpGet("{id:int}")]
        public async Task<User> Get(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            return user;
        }
        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<string> Token(string username,CancellationToken cancellationToken)
        {
            var user = await userRepository.FindUserByUserName(username, cancellationToken);
            if (user == null)
                throw new BadRequestException("این کاربر وجود ندارد");

            var exist = await userRepository.CheckUserNameAndPassword(user.UserName, user.Password, cancellationToken);
            if (!exist)
                throw new BadRequestException("رمز وارد شده اشتباه است");

            var jwt = jwtService.Generate(user);
            return jwt;
        }
        // POST api/<UserController>
        [HttpPost]
        public async Task<ApiResult<UserDto>> Post(UserDto model, CancellationToken cancellationToken)
        {
            var exisit = userRepository.TableNoTracking.Any(p => p.UserName == model.UserName);
            if (exisit)
                throw new BadRequestException("کاربر وجود ندارد.");
            var user = new User();
            user.Name = model.Name;
            user.UserName = model.UserName;
            user.Password = SecurityHelper.GetSha256Hash(model.Password);
            user.Family = model.Family;
            user.Age = model.Age;

            await userRepository.AddAsync(user, cancellationToken);
            var list = new List<string>();

            return Ok(model);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id:int}")]
        public async Task<ApiResult> Put(int id, UserDto user, CancellationToken cancellationToken)
        {
            var updateUser = await userRepository.GetByIdAsync(cancellationToken, id);
            updateUser.Name = user.Name;
            updateUser.UserName = user.UserName;
            updateUser.Password = SecurityHelper.GetSha256Hash(user.Password);
            updateUser.Family = user.Family;
            updateUser.Age = user.Age;

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
