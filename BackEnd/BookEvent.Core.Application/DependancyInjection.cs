﻿using BookEvent.Core.Application.Abstraction;
using BookEvent.Core.Application.Abstraction.Services.Auth;
using BookEvent.Core.Application.Abstraction.Services.Booking;
using BookEvent.Core.Application.Abstraction.Services.Categories;
using BookEvent.Core.Application.Abstraction.Services.Emails;
using BookEvent.Core.Application.Abstraction.Services.Events;
using BookEvent.Core.Application.Services.Auth;
using BookEvent.Core.Application.Services.Booking;
using BookEvent.Core.Application.Services.Categories;
using BookEvent.Core.Application.Services.Emails;
using BookEvent.Core.Application.Services.Events;
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
            services.AddScoped(typeof(IEventService), typeof(EventService));
            services.AddScoped(typeof(IBookService), typeof(BookService));

            services.AddAutoMapper(typeof(MappingProfile));


            services.AddScoped(typeof(Func<IAuthService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<IAuthService>();

            });

            services.AddScoped(typeof(Func<ICategoriesService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<ICategoriesService>();

            });

            services.AddScoped(typeof(Func<IEventService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<IEventService>();

            });

            services.AddScoped(typeof(Func<IBookService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<IBookService>();

            });
            return services;
        }
    }
}
