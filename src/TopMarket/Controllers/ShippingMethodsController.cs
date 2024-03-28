using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Categories;
using Service.DTOs.ShippingMethods;
using Service.Interfaces;
using TopMarket.Models;

namespace TopMarket.Controllers
{
    public class ShippingMethodsController : BaseController
    {
        private readonly IShippingMethodService shippingMethodService;
        public ShippingMethodsController(IShippingMethodService shippingMethodService)
        {
            this.shippingMethodService = shippingMethodService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> PostAsync(ShippingMethodCreationDto dto)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.shippingMethodService.CreateAsync(dto)
            });


        [HttpPut("update")]
        public async Task<IActionResult> PutAsync(ShippingMethodUpdateDto dto)
           => Ok(new Response
           {
               StatusCode = 200,
               Message = "Success",
               Data = await this.shippingMethodService.UpdateAsync(dto)
           });


        [HttpDelete("delete/{id:long}")]
        public async Task<IActionResult> DeleteAsync(long id)
           => Ok(new Response
           {
               StatusCode = 200,
               Message = "Success",
               Data = await this.shippingMethodService.DeleteAsync(id)
           });


        [HttpGet("get/{id:long}")]
        public async Task<IActionResult> GetByIdAsync(long id)
           => Ok(new Response
           {
               StatusCode = 200,
               Message = "Success",
               Data = await this.shippingMethodService.GetByIdAsync(id)
           });


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
           => Ok(new Response
           {
               StatusCode = 200,
               Message = "Success",
               Data = await this.shippingMethodService.GetAllAsync()
           });
    }
}
