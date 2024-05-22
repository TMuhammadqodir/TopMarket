using Microsoft.AspNetCore.Mvc;
using Service.DTOs.VariationOptions;
using Service.Interfaces;
using TopMarket.Models;

namespace TopMarket.Controllers;

public class VariationOptionsController : BaseController
{
    private readonly IVariationOptionService variationOptionService;
    public VariationOptionsController(IVariationOptionService variationOptionService)
    {
        this.variationOptionService = variationOptionService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> PostAsync(VariationOptionCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.variationOptionService.CreateAsync(dto)
        });


    [HttpPut("update")]
    public async Task<IActionResult> PutAsync(VariationOptionUpdateDto dto)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.variationOptionService.ModifyAsync(dto)
       });


    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.variationOptionService.RemoveAsync(id)
       });


    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.variationOptionService.RetrieveByIdAsync(id)
       });


    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync()
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.variationOptionService.RetrieveAllAsync()
       });
}
