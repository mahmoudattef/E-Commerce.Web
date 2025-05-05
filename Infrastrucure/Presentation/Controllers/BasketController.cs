using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransferObject.BasketModuleDtoss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize]
    public  class BasketController(IServiceManager _serviceManager): ApiBaseController
    {
        //Get Basket
        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasket(string Key) { 
        
            var basket =await _serviceManager.BasketService.GetBasketAsync(Key);
            return Ok(basket);
        }
        //Create Or Update
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket(BasketDto basket)
        {
            var Basket =await _serviceManager.BasketService.CreateOrUpdateBasketAsync(basket);
            return Ok(Basket);
        }
        //Delete
        [HttpDelete("{Key}")]
        public async Task<ActionResult<bool>> DeleteBasket(string Key) { 
            var Result =await _serviceManager.BasketService.DeleteBasketAsync(Key);
            return Ok(Result); 
        }
    }
}
