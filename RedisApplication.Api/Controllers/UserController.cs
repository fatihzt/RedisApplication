using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisApplication.Business.Request.User;
using RedisApplication.Business.Response.User;
using RedisApplication.Business.Service;
using RedisApplication.Core.Redis;
using RedisApplication.Entity;
using StackExchange.Redis;

namespace RedisApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRedisProvider _redisProvider;
        public UserController(IUserService userService,IRedisProvider redisProvider)
        {
            _userService = userService;
            _redisProvider= redisProvider;
                
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = _userService.GetAll();
            return Ok(result);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegistirationRequest user)
        {
            var result = _userService.GetAll(u => u.Name == user.Name && u.Surname == user.Surname);
            if (result.Any()) { return BadRequest("User is already exist."); }
            _userService.CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
            User entity = new()
            {
                Name = user.Name,
                Surname = user.Surname,
                Username= user.Username,
                Email = user.EMail,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            _userService.Add(entity);
            return Ok(entity);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginRequest user)
        {
            var result = _userService.GetAll(u => u.Email == user.EMail);
            if (!result.Any()) { return BadRequest("User is not found. Please Register"); }
            foreach (var item in result)
            {
                if (!_userService.VerifyPasswordHash(user.Password, item.PasswordHash, item.PasswordSalt)) return BadRequest("wrong info");
                string token = _userService.CreateToken(item);
                UserLoginResponse loginResponse = new() { EMail = user.EMail, Password = token };
                return Ok(loginResponse);
            }
            return Ok();
        }
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            User user=_redisProvider.GetUserFromCache(id);
            if(user == null) 
            { 
                user=_userService.GetUserById(id); 
                if(user!= null) { _redisProvider.CacheUser(user); }
            }
            if (user == null) { return NotFound(); }
            return Ok(user);
        }
    }
}
