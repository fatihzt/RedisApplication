using RedisApplication.Core.Abstract;
using RedisApplication.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisApplication.Business.Service
{
    public interface IUserService:IUserCore
    {
        User GetUserById(int userId);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        string CreateToken(User user);
    }
}
