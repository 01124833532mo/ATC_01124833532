using BookEvent.Core.Application.Abstraction;
using BookEvent.Core.Application.Abstraction.Services.Auth;
using BookEvent.Core.Application.Services.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookEvent.Core.Application
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IServiceManager), typeof(ServiceManager));
            services.AddScoped(typeof(IAuthService), typeof(AuthService));

            services.AddScoped(typeof(Func<IAuthService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<IAuthService>();

            });

            return services;
        }
    }
}
