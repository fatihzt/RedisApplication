using Newtonsoft.Json;
using RedisApplication.Entity;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisApplication.Core.Redis
{
    public interface IRedisProvider
    {
        string GetString(string key);
        void SetString(string key, string value, TimeSpan? expiry = null);
        bool Add(User entity);
        User GetUserFromCache(int userId);
        void CacheUser(User user);
    }

    public class RedisProvider : IRedisProvider
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly IDatabase _database;

        public RedisProvider()
        {
            var redisConnectionString = "localhost:6379"; // Redis bağlantı dizesini buraya girin
            ConfigurationOptions configOptions = ConfigurationOptions.Parse(redisConnectionString);
            configOptions.AbortOnConnectFail = false;
            _redisConnection = ConnectionMultiplexer.Connect(configOptions);
            _database = _redisConnection.GetDatabase();
        }

        public bool Add(User entity)
        {
            string serializedEntity = JsonConvert.SerializeObject(entity);
            return _database.StringSet("user:" + entity.Id, serializedEntity);
        }
        public User GetUserFromCache(int userId)
        {
            string cacheKey = "user:" + userId;
            string serializedUser = _database.StringGet(cacheKey);

            if (!string.IsNullOrEmpty(serializedUser))
            {
                return JsonConvert.DeserializeObject<User>(serializedUser);
            }

            return null;
        }
        public void CacheUser(User user)
        {
            string cacheKey = "user:" + user.Id;
            string serializedUser = JsonConvert.SerializeObject(user);
            _database.StringSet(cacheKey, serializedUser);
        }
        public string GetString(string key)
        {
            return _database.StringGet(key);
        }

        public void SetString(string key, string value, TimeSpan? expiry = null)
        {
            _database.StringSet(key, value, expiry);
        }
    }
}
