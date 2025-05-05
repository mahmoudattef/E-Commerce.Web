using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.ProductModule;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.DataTransferObject.ProductModuleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProductService(IUnitOFWork _unitOFWork,IMapper _mapper ) : IProductService
    {
        public async Task<IEnumerable<BrandDTo>> GetAllBrandsAsync()
        {
            var Repo = _unitOFWork.GetRepository<ProductBrand, int>();
            var Brands =await  Repo.GetAllAsync();
            var BrandsDto = _mapper.Map<IEnumerable<ProductBrand>,IEnumerable<BrandDTo>>(Brands);
            return BrandsDto;
        }


        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var Types=await _unitOFWork.GetRepository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(Types);
        }
        public async Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var Repo = _unitOFWork.GetRepository<Product, int>();
            var Specification = new ProductWithBrandAndTypeSpecifications(queryParams);
            var Products =await Repo.GetAllAsync(Specification);

            var Data = _mapper.Map<IEnumerable<Product>,IEnumerable<ProductDto>>(Products);
            var productCount = Data.Count();
            var CountSpec = new ProductCountSpecification(queryParams);
            var TotalCount = await Repo.CountAsync(CountSpec);
            return new PaginatedResult<ProductDto>(queryParams.PageIndex,productCount,TotalCount,Data);
        }

        public async Task<ProductDto> GetByIdAsync(int id)
            {
            var Specifications = new ProductWithBrandAndTypeSpecifications(id);
            var product =await _unitOFWork.GetRepository<Product, int>().GetByIdAsync(Specifications);

            if (product is null)
            {
                throw new ProductNotFoundException(id);
            }

            return _mapper.Map<Product, ProductDto>(product);
        }
    }
}
