using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    // Order [M:1]  DeliveryMethod
    // Order [1:M]  OrderItem
    public class Order: BaseEntity
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal,string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            PaymentIntentId= paymentIntentId;
            Items = items;
            SubTotal = subTotal;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now; // datetime for many countries
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; }
        //public int DeliveryMethodId { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; } // Navigation Property [one]
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>(); // Navigation Property [Many]
        public Decimal SubTotal { get; set; }
        //[NotMapped]
        //public Decimal Total => SubTotal + DeliveryMethod.Cost;
        public Decimal GetTotal()
            => SubTotal + DeliveryMethod.Cost;

        public string PaymentIntentId { get; set; }
    }
}
