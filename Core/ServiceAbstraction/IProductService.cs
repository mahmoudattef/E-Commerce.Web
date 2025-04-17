using Shared.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public  interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetByIdAsync(int id);
        Task<IEnumerable<BrandDTo>> GetAllBrandsAsync();
        Task<IEnumerable<TypeDto>> GetAllTypesAsync();


    }
}
