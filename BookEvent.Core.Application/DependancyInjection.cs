using BookEvent.Core.Application.Abstraction;
using BookEvent.Core.Application.Abstraction.Services.Auth;
using BookEvent.Core.Application.Abstraction.Services.Categories;
using BookEvent.Core.Application.Abstraction.Services.Emails;
using BookEvent.Core.Application.Services.Auth;
using BookEvent.Core.Application.Services.Categories;
using BookEvent.Core.Application.Services.Emails;
using BookEvent.Core.Application.Services.Mapping;
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
            services.AddSingleton(typeof(IEmailService), typeof(EmailService));
            services.AddScoped(typeof(ICategoriesService), typeof(CategoriesService));

            services.AddAutoMapper(typeof(MappingProfile));


            services.AddScoped(typeof(Func<IAuthService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<IAuthService>();

            });

            services.AddScoped(typeof(Func<ICategoriesService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<ICategoriesService>();

            });
            return services;
        }
    }
}
