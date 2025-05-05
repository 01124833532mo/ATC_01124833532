using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookEvent.Shared
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddSharedDependency(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
