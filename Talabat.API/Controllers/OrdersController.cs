using Talabat.API.Errors;
using Talabat.API.Helpers;
using Talabat.Core.AutoMapper;
using Talabat.Core.Dtos;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Specs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Talabat.API.Controllers
{
    [Authorize]
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail= User.FindFirstValue(ClaimTypes.Email);

            var order = await orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.ShippingAddress, orderDto.DeliveryMethodId);

            if (order is null) return BadRequest(new ApiResponse(400));

            return Ok(order);
        }

        [HttpGet]
        [Cached(6000)]
        public async Task<Pagination<OrderToReturnDto>> GetOrdersForUser([FromQuery]OrderSpecParams specParams)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var orders = await orderService.GetOrdersForSpecificUserAsync(buyerEmail, specParams);

            return orders;
        }

		[HttpGet("{id}")]
		[Cached(6000)]
		[ProducesResponseType(typeof(OrderToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUserAsync(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var order= await orderService.GetOrderByIdForUserAsync(id, buyerEmail);

            if(order is null) return NotFound(new ApiResponse(404));

            return new JsonResult(order);
        }


		[HttpGet("deliveryMethods")]
        [Cached(6000)]
        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAll()
            => await orderService.GetDeliveryMethodsAsync();
    }
}
