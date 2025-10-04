using AutoMapper;
using Talabat.Core.Dtos;
using Talabat.Core.Entities;
using Microsoft.Extensions.Configuration;

namespace Talabat.Core.AutoMapper
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{configuration["ApiUrl"]}{source.PictureUrl}";

            return string.Empty;
        }
    }
}
