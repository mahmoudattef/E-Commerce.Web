using DomainLayer.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
      class ProductWithBrandAndTypeSpecifications:BaseSpecifications<Product,int>
    {
        //Get All Product With Type And Brands
        public ProductWithBrandAndTypeSpecifications(ProductQueryParams  queryParams) :
            base(p=>(!queryParams.BrandId.HasValue|| p.BrandId==queryParams.BrandId)
            && (!queryParams.TypeId.HasValue || p.TypeId ==queryParams.TypeId)
            && (string.IsNullOrEmpty(queryParams.SearchValue) || p.Name.ToLower().Contains(queryParams.SearchValue.ToLower()) ))
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);

            switch (queryParams.SortingOptions) { 
                case  ProductSortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;

                case ProductSortingOptions.NameDesc: AddOrderByDescending(p => p.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;

                case ProductSortingOptions.PriceDesc:
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    break;


            }
                    ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }
        //Get product with Id
        public ProductWithBrandAndTypeSpecifications(int id):base(p=>p.Id==id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);

          
        }
    }
}
