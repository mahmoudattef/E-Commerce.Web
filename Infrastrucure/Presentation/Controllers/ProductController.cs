﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.DataTransferObject.ProductModuleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize]
    public class ProductController(IServiceManager _serviceManager) : ApiBaseController
    {
        
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProduct([FromQuery]ProductQueryParams queryParams) { 
            var products =await _serviceManager.ProductService.GetAllProductsAsync(queryParams);
            return Ok(products);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id) { 
            var product =await _serviceManager.ProductService.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetType()
        {
            var types =await _serviceManager.ProductService.GetAllTypesAsync();
            return Ok(types);

        }
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<BrandDTo>>> GetBrands()
        {
            var brans =await _serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(brans);
        }

    }
}
