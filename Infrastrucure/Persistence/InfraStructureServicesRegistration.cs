


using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Persistence
{
    public static class InfraStructureServicesRegistration
    {
        public static IServiceCollection AddInfraStrucureService(this IServiceCollection Services ,IConfiguration Configuration) {
            Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
           Services.AddScoped<IDataSeeding, DataSeeding>();
           Services.AddScoped<IUnitOFWork, UnitOfWork>();
            Services.AddScoped<IBasketRepository, BasketRepository>();
            Services.AddSingleton<IConnectionMultiplexer>( (_)=> 
            {
                return ConnectionMultiplexer.Connect(Configuration.GetConnectionString("RedisConnectionString"));
            }); 

            return Services;
        }
    }
}
