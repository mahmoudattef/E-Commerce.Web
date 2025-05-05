using DomainLayer.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Service.MappingProfiles;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class ApplicationServiceRegisteration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection Services)
        {

            Services.AddAutoMapper(typeof(ProductProfile).Assembly);
            Services.AddScoped<IServiceManager, ServiceManager>();
            return Services; 
        }
    }
}
