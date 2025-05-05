using Shared;
using Shared.DataTransferObject.ProductModuleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public  interface IProductService
    {
        Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams);
        Task<ProductDto> GetByIdAsync(int id);
        Task<IEnumerable<BrandDTo>> GetAllBrandsAsync();
        Task<IEnumerable<TypeDto>> GetAllTypesAsync();


    }
}
