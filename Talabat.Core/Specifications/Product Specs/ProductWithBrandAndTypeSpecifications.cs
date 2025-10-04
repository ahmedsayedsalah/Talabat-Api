using Talabat.Core.Entities;
using Talabat.Core.Specifications.Product_Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecifications: BaseSpecification<Product>
    {
        // Constructor: used for get all products with specs
        public ProductWithBrandAndTypeSpecifications(ProductSpecParams specParams)
            :base(p=> 
                      (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search))&&
                      (!specParams.BrandId.HasValue || p.ProductBrandId== specParams.BrandId.Value) &&
                      (!specParams.TypeId.HasValue || p.ProductTypeId == specParams.TypeId.Value)
            )
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);

            AddOrderBy(p => p.Name);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch(specParams.Sort)
                {
                    case "name":
						AddOrderBy(p => p.Name);
                        break;
					case "priceAsc":
                        AddOrderBy(p=> p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                }
            }

            ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
        }
        // Constructor: used for get specific products with specs
        public ProductWithBrandAndTypeSpecifications(int id):base(p=> p.Id==id)
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
        }
    }
}
