using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DataSeeding(StoreDbContext _dbContext ) : IDataSeeding
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
    }
}
