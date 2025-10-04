using Talabat.Core.Entities;
using Talabat.Core.Specifications.Product_Specs;
using Talabat.Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using AutoMapper;
using Talabat.Core.Services;
using Talabat.Core.Dtos;
using Talabat.Core.AutoMapper;

namespace Talabat.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<Pagination<ProductToReturnDto>> GetProductsAsync(ProductSpecParams specParams)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(specParams);
            var products = await unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            //map
            var productsDto = mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

            var countSpec = new ProductWithFilterationForCountSpecification(specParams);

            var count = await unitOfWork.Repository<Product>().GetCountWithSpecAsync(countSpec);

            return new Pagination<ProductToReturnDto>(specParams.PageIndex, specParams.PageSize, count, productsDto);
        }

        public async Task<ProductToReturnDto?> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(id);
            var product = await unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);

            //map
            var productDto = mapper.Map<ProductToReturnDto>(product);

            return productDto;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
            => await unitOfWork.Repository<ProductBrand>().GetAllAsync();

        public async Task<IReadOnlyList<ProductType>> GetTypesAsync()
            => await unitOfWork.Repository<ProductType>().GetAllAsync();

    }
}
