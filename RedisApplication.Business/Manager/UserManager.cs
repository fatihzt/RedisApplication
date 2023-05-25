using RedisApplication.Business.Service;
using RedisApplication.Core.Abstract;
using RedisApplication.Core.Redis;
using RedisApplication.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RedisApplication.Business.Manager
{
    public class UserManager : IUserService
    {
        private readonly IUserCore _userCore;
        private readonly RedisProvider _redisProvider;
        public UserManager(IUserCore userCore,RedisProvider redisProvider)
        {
            _userCore = userCore;
            _redisProvider = redisProvider;
        }

        public int Add(User entity)
        {
            bool addToRedis = _redisProvider.Add(entity);
            if (addToRedis)
            {
                return _userCore.Add(entity);
            }
            else
            {
                return _userCore.Add(entity);
            }
        }

        public bool Delete(User entity)
        {
            return (_userCore.Delete(entity));
        }

        public User Get(Expression<Func<User, bool>> filter = null)
        {
            return _userCore.Get(filter);
        }

        public List<User> GetAll(Expression<Func<User, bool>> filter = null)
        {
            return _userCore.GetAll(filter);
        }

        public bool Update(User entity)
        {
            return _userCore.Update(entity);
        }
    }
}
