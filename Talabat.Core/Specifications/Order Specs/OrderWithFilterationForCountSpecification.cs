using Talabat.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.Order_Specs
{
	public class OrderWithFilterationForCountSpecification: BaseSpecification<Order>
	{
		public OrderWithFilterationForCountSpecification(string buyerEmail,OrderSpecParams specParams):
			base(o => o.BuyerEmail == buyerEmail &&
			(!specParams.Status.HasValue || o.Status == specParams.Status!.Value)
			)
		{
			
		}
	}
}
