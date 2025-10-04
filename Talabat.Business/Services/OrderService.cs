using AutoMapper;
using Talabat.Core;
using Talabat.Core.AutoMapper;
using Talabat.Core.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Specs;
using Microsoft.Extensions.Configuration;

namespace Talabat.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IPaymentService paymentService;
        private readonly IConfiguration configuration;

        public OrderService(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPaymentService paymentService,
            IConfiguration configuration
            )
        {
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.paymentService = paymentService;
            this.configuration = configuration;
        }
        public async Task<OrderToReturnDto?> CreateOrderAsync(string buyerEmail, string basketId, AddressDto addressDto, int deliveryMethodId)
        {
            // 1. Get Basket From Baskets Repo
            var basket = await basketRepository.GetBasketAsync(basketId);

            // 2. Get Selected Items at Basket(Id,Quantity) From Product Repo
            var orderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                orderItems.Add(orderItem);
            }

            // 3. Calculate SubTotal
            var subTotal = orderItems.Sum(ot => ot.Quantity * ot.Price);

            // 4. Get DeliveryMethod From DeliveryMethod Repo  
            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            basket.ShippingCost= deliveryMethod.Cost;

            // 5. Map AddressDto to Address
            var address = mapper.Map<Address>(addressDto);


            // validate more order in one basket
            var spec = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);
            var existOrder = await unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if (existOrder is not null)
            {
                unitOfWork.Repository<Order>().Delete(existOrder);
                await paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }

            // 6. Create Order
            var order = new Order(buyerEmail, address, deliveryMethod, orderItems, subTotal,basket.PaymentIntentId);

            // 7. Save To DB 
            await unitOfWork.Repository<Order>().Add(order);

            var affectedCount = await unitOfWork.CompleteAsync();

            return affectedCount > 0 ? mapper.Map<OrderToReturnDto>(order) : null;
        }

        public async Task<OrderToReturnDto?> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var specs = new OrderSpecification(orderId, buyerEmail);
            var order = await unitOfWork.Repository<Order>().GetEntityWithSpecAsync(specs);
            var orderDto= mapper.Map<OrderToReturnDto>(order);

            return orderDto;
        }

        public async Task<Pagination<OrderToReturnDto>> GetOrdersForSpecificUserAsync(string buyerEmail,OrderSpecParams specParams)
        {
            var specs = new OrderSpecification(buyerEmail, specParams);
            var orders = await unitOfWork.Repository<Order>().GetAllWithSpecAsync(specs);

            var ordersDto = mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);

            var countSpecs = new OrderWithFilterationForCountSpecification(buyerEmail, specParams);
            var count = await unitOfWork.Repository<Order>().GetCountWithSpecAsync(countSpecs);

            return new Pagination<OrderToReturnDto>(specParams.PageIndex,specParams.PageSize,count,ordersDto);
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
            => await unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

    }
}
