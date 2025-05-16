using BookEvent.Core.Application.Abstraction.Common.Contracts.Infrastracture;
using BookEvent.Infrastructure.AttachementService;
using BookEvent.Infrastructure.Caching_Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BookEvent.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped(typeof(IAttachmentService), typeof(AttachmentService));
            services.AddSingleton(typeof(IResponseCacheService), typeof(ResponseCacheService));

            services.AddSingleton(typeof(IConnectionMultiplexer), (serviceprovider) =>
            {
                var connectionstring = configuration.GetConnectionString("Redis");
                var multiplexer = ConnectionMultiplexer.Connect(connectionstring!);
                return multiplexer;

            });

            return services;
        }
    }
}
