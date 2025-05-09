﻿using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddleWares;

namespace E_Commerce.Web.Extensions
{
    public static class WebApplicationRegistration
    {
        public static async Task SeedDataBaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var ObjDataSeed = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            await ObjDataSeed.DataSeedAsync();
            await ObjDataSeed.IdentityDataSeedAsync();



        }

        public static IApplicationBuilder UseCustomExceptionMiddlelWare(this IApplicationBuilder app) {
            app.UseMiddleware<CustomExceptionHandlerMiddleWare>();
            
            return app;
        }
        public static IApplicationBuilder UseSwaggerMiddleWares(this IApplicationBuilder app) {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
