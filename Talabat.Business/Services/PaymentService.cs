using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Specs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Business.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<PaymentService> logger;

        public PaymentService(
            IConfiguration configuration,
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            ILogger<PaymentService> logger
            )
        {
            this.configuration = configuration;
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = configuration["StripeSettings:Secretkey"];
            var basket= await basketRepository.GetBasketAsync(basketId);

            if (basket is null) return null;

            var shippingPrice = 0m;

            if(basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
                basket.ShippingCost= deliveryMethod.Cost;
            }

            if(basket?.Items?.Count>0)
            {
                foreach(var item in basket.Items)
                {
                    var product = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if(item.Price != product.Price)
                        item.Price = product.Price;
                }
            }

            var services=new PaymentIntentService();
            PaymentIntent paymentIntent;
            if(string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount= (long)((basket.Items.Sum(i => i.Quantity * i.Price) + shippingPrice) * 100),
                    Currency = "usd",
                    PaymentMethodTypes= new List<string>() { "card" }
                };

               paymentIntent= await services.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(i => i.Quantity * i.Price * 100) + (long)shippingPrice * 100
                };
                await services.UpdateAsync(basket.PaymentIntentId, options);
            }

            await basketRepository.UpdateBasketAsync(basket);

            return basket;
        }

        public async Task<Order> UpdatePaymentToSucceededOrDefault(string paymentIntentId, bool isSucceed)
        {
            var spec = new OrderWithPaymentIntentSpecification(paymentIntentId);
            var order= await unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);

            if(isSucceed)
                order.Status= OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;

            unitOfWork.Repository<Order>().Update(order);
            await unitOfWork.CompleteAsync();

            return order;
        }
    }
}
