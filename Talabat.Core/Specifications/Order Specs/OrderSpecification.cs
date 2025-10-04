using Talabat.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.Order_Specs
{
    public class OrderSpecification: BaseSpecification<Order>
    {
        // Constructor: to Endepoint GetOrdersForUser
        public OrderSpecification(string buyerEmail,OrderSpecParams specParams)
            :base(o=> o.BuyerEmail== buyerEmail&&
			(!specParams.Status.HasValue || o.Status == specParams.Status!.Value))
        {
            Includes.Add(o=> o.DeliveryMethod);
            Includes.Add(o=> o.Items);

            switch (specParams.Sort) {
                case "priceAsc":
                    AddOrderBy(o => o.SubTotal+o.DeliveryMethod.Cost);
                    break;
                case "priceDesc":
                    AddOrderByDesc(o => o.SubTotal + o.DeliveryMethod.Cost);
                    break;
                case "dateAsc":
                    AddOrderBy(o => o.OrderDate);
                    break;
                case "dateDesc":
                    AddOrderByDesc(o => o.OrderDate);
                    break;
            }

            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }
        // Constructor: to Endepoint GetOrderForUserAsync
        public OrderSpecification(int id,string buyerEmail)
            :base(o=>
                   o.Id== id &&
                   o.BuyerEmail == buyerEmail
                 )
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }
    }
}
