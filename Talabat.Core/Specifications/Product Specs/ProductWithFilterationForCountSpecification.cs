using Talabat.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.Product_Specs
{
    public class ProductWithFilterationForCountSpecification: BaseSpecification<Product>
    {
        public ProductWithFilterationForCountSpecification(ProductSpecParams specParams)
            : base(p=>
					   (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search)) &&
					   (!specParams.BrandId.HasValue || p.ProductBrandId==specParams.BrandId)&&
                       (!specParams.TypeId.HasValue || p.ProductTypeId == specParams.TypeId)
                  )
        {
            
        }
    }
}
