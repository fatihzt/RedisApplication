using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisApplication.Business.Service;
using RedisApplication.Entity;
using StackExchange.Redis;

namespace RedisApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = _userService.GetAll();
            return Ok(result);
        }
        //[HttpPost]
        //public async Task<IActionResult> Register()
        //{
        //    User entity = new() { Id = 1, Name = "a", Surname = "b", Email = "c" };
        //    var result = _userService.Add(entity);
        //    return Ok(result);
        //}
    }
}
