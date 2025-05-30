﻿using BookEvent.Apis.Controller;
using BookEvent.Apis.Services;
using BookEvent.Core.Application.Abstraction;
using BookEvent.Shared.Errors.Response;
using Microsoft.AspNetCore.Mvc;

public static class DependencyInjection
{

    public static IServiceCollection RegesteredPresestantLayer(this IServiceCollection services)
    {


        services.AddControllers().ConfigureApiBehaviorOptions((option) =>
        {
            option.SuppressModelStateInvalidFilter = false;
            option.InvalidModelStateResponseFactory = (action) =>
            {
                var errors = action.ModelState.
                Where(p => p.Value!.Errors.Count > 0)
                .SelectMany(e => e.Value!.Errors).Select(e => e.ErrorMessage);

                return new BadRequestObjectResult(new ApiValidationErrorResponse() { Erroes = errors });
            };
        }).AddApplicationPart(typeof(AssemblyInformation).Assembly);
        #region CORS
        // To Do Allow SpecificOrigins
        services.AddCors(corsOptions =>
        {
            corsOptions.AddPolicy("BookEvent", policyBuilder =>
            {
                policyBuilder.AllowAnyHeader()
                             .AllowAnyMethod()
                             .AllowAnyOrigin(); // Allow all domains
            });
        });

        #endregion

        services.AddScoped(typeof(ILoggedInUserService), typeof(LoggedInUserService));
        services.AddHttpContextAccessor();
        return services;
    }

}