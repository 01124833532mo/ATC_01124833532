using BookEvent.Apis.Extentions;
using BookEvent.Apis.MiddleWares;
using BookEvent.Core.Application;
using BookEvent.Infrastructure;
using BookEvent.Infrastructure.Persistence;
using BookEvent.Shared;

namespace BookEvent.Apis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.RegesteredPresestantLayer();
            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddSharedDependency(builder.Configuration);


            var app = builder.Build();
            await app.InitializerBookEventContextAsync();

            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExeptionHandlerMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStatusCodePagesWithReExecute("/Errors/{0}");
            app.UseStaticFiles();

            app.UseCors("BookEvent");


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
