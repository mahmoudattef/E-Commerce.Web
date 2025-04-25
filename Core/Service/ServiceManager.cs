using AutoMapper;
using DomainLayer.Contracts;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManager(IUnitOFWork _unitOFWork, IMapper _mapper ,IBasketRepository basketRepository) : IServiceManager
    {
        private readonly Lazy<IProductService> _LazyProductService = new Lazy<IProductService>(() => new ProductService(_unitOFWork, _mapper));
        public IProductService ProductService => _LazyProductService.Value;
        private readonly Lazy<IBasketService> _LazyBasketService = new Lazy<IBasketService>(() =>new BasketService (basketRepository, _mapper));
        public IBasketService BasketService => _LazyBasketService.Value;
    }
}
