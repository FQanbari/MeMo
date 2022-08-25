using Common.Utilities;
using Memo.App.Data.IRepository;
using Memo.App.Entities.Account;
using Memo.App.WebFramework.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Memo.App.WebFramework.Filter;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Memo.App.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultAtribute]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        // GET: api/<UserController>
        [HttpGet]
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

        public class UserViewModel
        {
            public string UserName { get; set; }
            public string Password { get; set; }

            public string Name { get; set; }
            public string Family { get; set; }
            public int Age { get; set; }
            public bool IsActive { get; set; }
        }
        // POST api/<UserController>
        [HttpPost]
        public async Task<ApiResult<UserViewModel>> Post(UserViewModel model, CancellationToken cancellationToken)
        {
            var user = new User();
            user.Name = model.Name;
            user.UserName = model.UserName;
            user.Password = SecurityHelper.GetSha256Hash(model.Password);
            user.Family = model.Family;
            user.Age = model.Age;

            await userRepository.AddAsync(user, cancellationToken);

            return Ok(model);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id:int}")]
        public async Task<ApiResult> Put(int id, UserViewModel user, CancellationToken cancellationToken)
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
