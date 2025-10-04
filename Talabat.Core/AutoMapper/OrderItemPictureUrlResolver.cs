using AutoMapper;
using AutoMapper.Execution;
using Talabat.Core.Dtos;
using Talabat.Core.Entities.Order_Aggregate;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.AutoMapper
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty((source.Product.PictureUrl)))
                return $"{configuration["ApiUrl"]}{source.Product.PictureUrl}";

            return string.Empty;
        }
    }
}
