using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisApplication.Business.Service;
using StackExchange.Redis;

namespace RedisApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result=_userService.GetAll(a=>a.Id==0);
            return Ok(result);
        }
    }
}
