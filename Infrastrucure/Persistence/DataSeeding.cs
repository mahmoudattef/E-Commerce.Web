using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DataSeeding(StoreDbContext _dbContext ,UserManager<ApplicationUser> _userManager,
        RoleManager<IdentityRole> _roleManager,
        StoreIdentityDbContext _identityDbContext) : IDataSeeding
    {
        public async Task DataSeedAsync()
        {
            try
            {
                if ((await _dbContext.Database.GetPendingMigrationsAsync()).Any())
                {
                   await _dbContext.Database.MigrateAsync(); // applay all migrations not update-database

                }

                if (!_dbContext.ProductBrands.Any())
                {
                    //Read data
                    //var productBrandData = await File.ReadAllTextAsync(@"..\Infrastrucure\Persistence\Data\DataSeed\brands.json");
                    var productBrandData = File.OpenRead(@"..\Infrastrucure\Persistence\Data\DataSeed\brands.json");

                    //Convert Data from string to c# object ==> Deserialize 
                    var productBrands =await JsonSerializer.DeserializeAsync<List<ProductBrand>>(productBrandData);
                    //Save To DataBase
                    if (productBrands is not null && productBrands.Any() )
                    {

                       await _dbContext.AddRangeAsync(productBrands);
                    }
                }
                if (!_dbContext.ProductTypes.Any())
                {
                    var productTypesData = File.OpenRead(@"..\Infrastrucure\Persistence\Data\DataSeed\types.json");
                    //Convert Data from string to c# object ==> Deserialize 
                    var productTypes =await  JsonSerializer.DeserializeAsync<List<ProductType>>(productTypesData);
                    if (productTypes is not null &&productTypes.Any() )
                    {

                       await _dbContext.AddRangeAsync(productTypes);
                    }
                }
                if (!_dbContext.Products.Any())
                {
                    var productsData = File.OpenRead(@"..\Infrastrucure\Persistence\Data\DataSeed\products.json");
                    //Convert Data from string to c# object ==> Deserialize 
                    var products =await JsonSerializer.DeserializeAsync<List<Product>>(productsData);
                    if (products is not null && products.Any() )
                    {

                       await _dbContext.AddRangeAsync(products);
                    }
                }

                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                //ToDo
            }
        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));

                }
                if (!_userManager.Users.Any())
                {
                    var User01 = new ApplicationUser()
                    {
                        Email = "Mahmoud@gmail.com",
                        DisplayName = "Mahmoud Atef",
                        PhoneNumber = "01145752170",
                        UserName = "MahmoudAtef"
                    };
                    var User02 = new ApplicationUser()
                    {
                        Email = "Mohamed@gmail.com",
                        DisplayName = "Mohamed Atef",
                        PhoneNumber = "01145752170",
                        UserName = "MohamedAtef"
                    };
                    await _userManager.CreateAsync(User01, "P@ssw0rd");
                    await _userManager.CreateAsync(User02, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(User01, "SuperAdmin");

                    await _userManager.AddToRoleAsync(User02, "Admin");

                }
               await _identityDbContext.SaveChangesAsync();
            }
            catch (Exception ex) { 
            }
        }
    }
}
