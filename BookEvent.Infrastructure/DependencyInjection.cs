using BookEvent.Core.Application.Abstraction.Common.Contracts.Infrastracture;
using BookEvent.Infrastructure.AttachementService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookEvent.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped(typeof(IAttachmentService), typeof(AttachmentService));

            return services;
        }
    }
}
