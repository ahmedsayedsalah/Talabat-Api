using AutoMapper;
using Talabat.API.Errors;
using Talabat.Core.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Talabat.API.Controllers
{
    //[Authorize]
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService pamentService;
        private const string _whSecret = "whsec_d99906888be64a5325a26e4aec06d42fe9bf71953e16de3e69897fd02d07077c";

		public PaymentsController(IPaymentService pamentService)
        {
            this.pamentService = pamentService;
        }

		[Authorize]
		[ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await pamentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket is null) return BadRequest(new ApiResponse(400,"A problem with your basket"));

            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _whSecret);
            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
            Order order;

            switch(stripeEvent.Type)
            {
                case EventTypes.PaymentIntentSucceeded:
                    await pamentService.UpdatePaymentToSucceededOrDefault(paymentIntent.Id, true);
                    break;
                case EventTypes.PaymentIntentPaymentFailed:
                    await pamentService.UpdatePaymentToSucceededOrDefault(paymentIntent.Id, false);
                    break;
            }

            return Ok();
        }
    }
}
