using BookEvent.Apis.MiddleWares;
using BookEvent.Core.Application;
using BookEvent.Infrastructure;
using BookEvent.Infrastructure.Persistence;

namespace BookEvent.Apis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.RegesteredPresestantLayer();
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddPersistenceServices(builder.Configuration);


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExeptionHandlerMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStatusCodePagesWithReExecute("/Errors/{0}");


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
