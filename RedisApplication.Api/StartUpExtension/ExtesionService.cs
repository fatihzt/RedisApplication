using RedisApplication.Business.Manager;
using RedisApplication.Business.Service;
using RedisApplication.Core.Abstract;
using RedisApplication.Core.Concrete;
using RedisApplication.Core.Redis;
using StackExchange.Redis;

namespace RedisApplication.Api.StartUpExtension
{
    public static class ExtensionService
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserManager>();
            services.AddScoped<IUserCore, EfUserCore>();
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<IProductCore, EfProductCore>();
            services.AddScoped<IOrderService, OrderManager>();
            services.AddScoped<IOrderCore, EfOrderCore>();
            services.AddScoped<IOrderItemService, OrderItemManager>();
            services.AddScoped<IOrderItemCore, EfOrderItemCore>();
            services.AddScoped<IRedisProvider, RedisProvider>();
            services.AddScoped<RedisProvider>();

            services.AddSingleton<ConnectionMultiplexer>(provider =>
            {
                var redisConnectionString = "localhost:6379"; // Redis bağlantı dizesini buraya girin
                return ConnectionMultiplexer.Connect(redisConnectionString);
            });

            services.AddScoped<IDatabase>(provider =>
            {
                var redisConnection = provider.GetRequiredService<ConnectionMultiplexer>();
                return redisConnection.GetDatabase();
            });


        }
    }
}
