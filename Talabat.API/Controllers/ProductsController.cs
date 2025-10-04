using AutoMapper;
using Talabat.API.Errors;
using Talabat.API.Helpers;
using Talabat.Core.AutoMapper;
using Talabat.Core.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Product_Specs;
using Microsoft.AspNetCore.Mvc;

namespace Talabat.API.Controllers
{
    public class ProductsController : ApiBaseController
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }
        //[Authorize/*(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)*/]
        [HttpGet]
        [Cached(6000)]
        public async Task<Pagination<ProductToReturnDto>> GetProducts([FromQuery]ProductSpecParams specParams)
            => await productService.GetProductsAsync(specParams);

		[HttpGet("{id}")]
		[Cached(6000)]
		[ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
		{
			var product = await productService.GetProductByIdAsync(id);

			if (product is null) return NotFound(new ApiResponse(400));

			return new JsonResult(product);
		}

		[HttpGet("brands")]
        [Cached(6000)]
        public async Task<IReadOnlyList<ProductBrand>> GetBrands()
            => await productService.GetBrandsAsync();

        [HttpGet("types")]
        [Cached(6000)]
        public async Task<IReadOnlyList<ProductType>> GetTypes()
            => await productService.GetTypesAsync();
    }
}
