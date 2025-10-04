using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Talabat.DataAccess.Reposioties
{
    public class BasketRepository : IBasketRepository
    {
        private IDatabase database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            database= redis.GetDatabase();
        }

        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var basket = await database.StringGetAsync(BasketId);

            return (basket.IsNull)? null: JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var createdOrUpdated = await database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(1));

            if (!createdOrUpdated) return null;
            return await GetBasketAsync(basket.Id);
        }

        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
            return await database.KeyDeleteAsync(BasketId);
        }
    }
}
