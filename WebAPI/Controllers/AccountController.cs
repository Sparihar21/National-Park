using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data.Repository.IRepository;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]User user)
        {
            if (ModelState.IsValid)
            {
                var isUniqueUser = _userRepository.IsUniqueUserName(user.UserName);
                if (!isUniqueUser) return BadRequest("User is in db");
                var userInDb = _userRepository.Register(user.UserName, user.Password);
                if (userInDb == null) return BadRequest();
            }
            return Ok();
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]UserNameVM userNameVM)
        {
            var user = _userRepository.Autehnticate(userNameVM.UserName, userNameVM.UserPassword);
            if (user == null) return BadRequest("Wrong UN/P");
            return Ok(user);
        }
    }
}
