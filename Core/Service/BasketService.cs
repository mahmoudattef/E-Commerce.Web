using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.BasketModule;
using ServiceAbstraction;
using Shared.DataTransferObject.BasketModuleDtoss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Service
{
    public class BasketService(IBasketRepository _basketRepository,IMapper _mapper) : IBasketService

    {
        public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basket)
        {
            var CustomerBasket =_mapper.Map<BasketDto,CustomerBasket>(basket);
          var IsCreatedOrUpdated= await _basketRepository.CreateOrUpdateBasketAsync(CustomerBasket);
            if (IsCreatedOrUpdated is not null)
                return await GetBasketAsync(basket.Id);
            else
                throw new Exception("Can Not Update Or Create Basket Now");
        }

        public async Task<bool> DeleteBasketAsync(string Key) =>await _basketRepository.DeleteBasketAsync(Key);
        public async Task<BasketDto> GetBasketAsync(string Key)
        {
            var Basket =await _basketRepository.GetBasketAsync(Key);
            if (Basket is not null)
            {
                return _mapper.Map<CustomerBasket, BasketDto>(Basket);

            }
            else throw new BasketNotFoundException(Key);
        }
    }
}
