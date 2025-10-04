using Talabat.Core.AutoMapper;
using Talabat.Core.Dtos;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Specifications.Order_Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Services
{
    public interface IOrderService
    {
        Task<OrderToReturnDto?> CreateOrderAsync(string buyerEmail,string basketId,AddressDto address,int deliveryMethodId);
        Task<Pagination<OrderToReturnDto>> GetOrdersForSpecificUserAsync(string buyerEmail, OrderSpecParams specParams);
        Task<OrderToReturnDto?> GetOrderByIdForUserAsync(int orderId, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
