using BookEvent.Core.Domain.Contracts.Persestence;
using BookEvent.Core.Domain.Contracts.Persestence.DbInitializers;
using BookEvent.Infrastructure.Persistence._Data;
using BookEvent.Infrastructure.Persistence._Data.Interceptors;
using BookEvent.Infrastructure.Persistence.Repositories.Generic_Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookEvent.Infrastructure.Persistence
{
    public static class DependancyInjection
    {

        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<BookEventDbContext>((provider, options) =>
            {
                options.UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("BookEventContext"))
                .AddInterceptors(provider.GetRequiredService<AuditInterceptor>());

            });
            services.AddScoped(typeof(AuditInterceptor));
            services.AddScoped(typeof(IBookEventDbInitializer), typeof(BookEventDbInitilzer));
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork.UnitOfWork));


            return services;
        }
    }
}
