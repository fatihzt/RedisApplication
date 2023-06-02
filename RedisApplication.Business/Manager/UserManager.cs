using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RedisApplication.Business.Service;
using RedisApplication.Core.Abstract;
using RedisApplication.Core.Redis;
using RedisApplication.Entity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RedisApplication.Business.Manager
{
    public class UserManager : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserCore _userCore;
        private readonly RedisProvider _redisProvider;
        public UserManager(IUserCore userCore,RedisProvider redisProvider, IConfiguration configuration)
        {
            _userCore = userCore;
            _redisProvider = redisProvider;
            _configuration = configuration;
        }

        public int Add(User entity)
        {
            return _userCore.Add(entity);
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null || password.Length < 9) { throw new ArgumentException("Password must be at least 8 characters long"); }
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public string CreateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var claims = new[]
            {
        new Claim("Name", user.Name),
        new Claim("Id", user.Id.ToString()),
        new Claim("Surname", user.Surname),
    };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims: claims,
                signingCredentials: creds,
                expires: DateTime.Now.AddMinutes(10)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
        //public string CreateToken(User user)
        //{
        //    var secureKey = Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value);
        //    var claims = new[]
        //            {
        //                    new Claim("Name",user.Name),
        //                    new Claim("Id",user.Id.ToString()),
        //                    new Claim("PasswordHash",user.PasswordHash.ToString()),
        //                    new Claim("PasswordSalt",user.PasswordSalt.ToString()),
        //                    new Claim("Surname",user.Surname),
        //                };

        //    var securityKey = new SymmetricSecurityKey(secureKey);
        //    var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);


        //    var token = new JwtSecurityToken(
        //        _configuration.GetSection("Jwt:Issuer").Value,
        //        _configuration.GetSection("Jwt:Audience").Value,
        //        claims: claims,
        //        signingCredentials: creds,
        //        expires: DateTime.Now.AddMinutes(10)

        //        );

        //    var tokens = new JwtSecurityTokenHandler().WriteToken(token);

        //    return tokens;
        //}

        public bool Delete(User entity)
        {
            return _userCore.Delete(entity);
        }

        public User Get(Expression<Func<User, bool>> filter = null)
        {
            return _userCore.Get(filter);
        }

        public List<User> GetAll(Expression<Func<User, bool>> filter = null)
        {
            return _userCore.GetAll(filter);
        }

        public User GetUserById(int userId)
        {
            User user = _redisProvider.GetUserFromCache(userId);

            if (user == null)
            {
                user = _userCore.Get(u => u.Id == userId);

                if (user != null)
                {
                    _redisProvider.CacheUser(user);
                }
            }

            return user;
        }

        public bool Update(User entity)
        {
            return _userCore.Update(entity);
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}

            //bool addToRedis = _redisProvider.Add(entity);
            //if (addToRedis)
            //{
            //    return _userCore.Add(entity);
            //}
            //else
            //{
            //    return _userCore.Add(entity);
            //}