using DomainLayer.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    class ProductCountSpecification : BaseSpecifications<Product, int>
    {
        public ProductCountSpecification(ProductQueryParams queryParams) :  base(p=>(!queryParams.BrandId.HasValue|| p.BrandId==queryParams.BrandId)
            && (!queryParams.TypeId.HasValue || p.TypeId ==queryParams.TypeId)
            && (string.IsNullOrEmpty(queryParams.SearchValue) || p.Name.ToLower().Contains(queryParams.SearchValue.ToLower()) ))
        {

        }
    }
}
