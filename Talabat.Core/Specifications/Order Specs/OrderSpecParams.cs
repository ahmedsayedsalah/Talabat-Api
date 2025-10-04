
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Specs
{
	public class OrderSpecParams
	{
		private const int MaxPageSize = 10;
		private int pageSize = 5;

		public int PageSize
		{
			get { return pageSize; }
			set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
		}

		public int PageIndex { get; set; } = 1;
		public string? Sort { get; set; }
		public OrderStatus? Status { get; set; }
	}
}

