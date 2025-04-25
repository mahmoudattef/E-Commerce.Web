
using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddleWares;
using E_Commerce.Web.Extensions;
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Persistence.Repositories;
using Service;
using Service.MappingProfiles;
using ServiceAbstraction;
using Shared.ErrorModels;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            
            builder.Services.AddSwaggerServices();

            builder.Services.AddInfraStrucureService(builder.Configuration);
            builder.Services.AddApplicationService();
            
            //Validation Error Response
             builder.Services.AddWebApplicationService();
            var app = builder.Build();
            //Data Seed
            await app.SeedDataBaseAsync();


            // Configure the HTTP request pipeline.
            app.UseCustomExceptionMiddlelWare();
            if (app.Environment.IsDevelopment())
            { 
                app.UseSwaggerMiddleWares();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
