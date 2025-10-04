using AutoMapper;
using Talabat.API.Errors;
using Talabat.Core.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Talabat.API.Controllers
{
    public class BasketsController : ApiBaseController
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;

        public BasketsController(IBasketRepository basketRepository,IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.mapper = mapper;
        }

        [HttpGet] // api/Baskets?id=basket1
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string id)
        {
            var basket= await basketRepository.GetBasketAsync(id);

            return basket is null ? new CustomerBasket(id) : Ok(basket);
        }

        [HttpPost] // api/baskets
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdateBasket(CustomerBasketDto basket) //for validation (CustomerBasketDto)
        {
            var mappedBasket= mapper.Map<CustomerBasket>(basket);
            var createdOrUpdatedBasket= await basketRepository.UpdateBasketAsync(mappedBasket);

            return basket is null? BadRequest(new ApiResponse(400)): Ok(createdOrUpdatedBasket);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            return await basketRepository.DeleteBasketAsync(id);
        }
    }
}
