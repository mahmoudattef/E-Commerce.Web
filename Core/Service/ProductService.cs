using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Models;
using ServiceAbstraction;
using Shared.DataTransferObject;
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

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var Products =await _unitOFWork.GetRepository<Product, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<Product>,IEnumerable<ProductDto>>(Products);
        }

        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var Types=await _unitOFWork.GetRepository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(Types);
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product =await _unitOFWork.GetRepository<Product, int>().GetByIdAsync(id);
            return _mapper.Map<Product, ProductDto>(product);
        }
    }
}
