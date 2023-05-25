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
    }

    public class RedisProvider : IRedisProvider
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly IDatabase _database;

        public RedisProvider()
        {
            var redisConnectionString = "localhost:6379"; // Redis bağlantı dizesini buraya girin
            _redisConnection = ConnectionMultiplexer.Connect(redisConnectionString);
            _database = _redisConnection.GetDatabase();
        }

        public bool Add(User entity)
        {
            string serializedEntity = JsonConvert.SerializeObject(entity);
            return _database.StringSet("user:" + entity.Id, serializedEntity);
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
