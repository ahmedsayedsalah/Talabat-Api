using Talabat.Core.Entities;

namespace Talabat.Core.Dtos
{
    public class CustomerBasketDto
    {
        public string Id { get; set; }
        public IList<BasketItemDto> Items { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingCost { get; set; }
    }
}
