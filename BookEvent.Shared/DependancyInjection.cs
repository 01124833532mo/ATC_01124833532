using BookEvent.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookEvent.Shared
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddSharedDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            return services;
        }
    }
}
