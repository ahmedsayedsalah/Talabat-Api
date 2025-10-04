using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Talabat.DataAccess.Data
{
    public static class SeedAppDbContext
    {
        public async static Task SeedAsync(AppDbContext dbContext)
        {
            if(!dbContext.ProductBrands.Any())
            {
                var brandsJson = File.ReadAllText("../Talabat.DataAccess/Data/DataSeed/brands.json");
                var barnds= JsonSerializer.Deserialize<IList<ProductBrand>>(brandsJson);

                if(barnds?.Count > 0)
                {
                    await dbContext.ProductBrands.AddRangeAsync(barnds);
                    await dbContext.SaveChangesAsync();
                }

            }
            if (!dbContext.ProductType.Any())
            {
                var typesJson = File.ReadAllText("../Talabat.DataAccess/Data/DataSeed/types.json");
                var types = JsonSerializer.Deserialize<IList<ProductType>>(typesJson);

                if (types?.Count > 0)
                {
                    await dbContext.ProductType.AddRangeAsync(types);
                    await dbContext.SaveChangesAsync();
                }

            }
            if (!dbContext.Products.Any())
            {
                var productsJson = File.ReadAllText("../Talabat.DataAccess/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<IList<Product>>(productsJson);

                if (products?.Count > 0)
                {
                    await dbContext.Products.AddRangeAsync(products);
                    await dbContext.SaveChangesAsync();
                }

            }
            if(!dbContext.DeliveryMethods.Any())
            {
                var deliveryMethodsJson = File.ReadAllText("../Talabat.DataAccess/Data/DataSeed/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<IList<DeliveryMethod>>(deliveryMethodsJson);

                if(deliveryMethods?.Count > 0)
                {
                    await dbContext.DeliveryMethods.AddRangeAsync(deliveryMethods);
                    await dbContext.SaveChangesAsync();
                }
            }

        }
    }
}
