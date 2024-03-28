using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Attachments;
using Service.DTOs.ProductItems;
using Service.Interfaces;
using TopMarket.Models;

namespace TopMarket.Controllers;

public class ProductItemsController : BaseController
{
    private readonly IProductItemService productItemService;
    public ProductItemsController(IProductItemService productItemService)
    {
        this.productItemService = productItemService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> PostAsync(ProductItemCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.productItemService.CreateAsync(dto)
        });


    [HttpPatch("Add")]
    public async Task<IActionResult> AddAsync(ProductItemIncomeDto dto)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.productItemService.AddAsync(dto)
       });


    [HttpPatch("Substract")]
    public async Task<IActionResult> SubstractAsync(ProductItemIncomeDto dto)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.productItemService.SubstractAsync(dto)
       });


    [HttpPut("update")]
    public async Task<IActionResult> PutAsync(ProductItemUpdateDto dto)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.productItemService.UpdateAsync(dto)
       });


    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.productItemService.DeleteAsync(id)
       });


    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.productItemService.GetByIdAsync(id)
       });


    [HttpGet("get-by-productId/{productId:long}")]
    public async Task<IActionResult> GetByProductIdAsync(long productId)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.productItemService.GetByProductIdAsync(productId)
       });


    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync()
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.productItemService.GetAllAsync()
       });


    [HttpPost("add-image")]
    public async Task<IActionResult> ImageUploadAsync(long productItemId, [FromForm] AttachmentCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.productItemService.AddImageAsync(productItemId, dto)
        });

    [HttpDelete("delete-image")]
    public async Task<IActionResult> DeleteImageAsync(long imageId, long productItemId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.productItemService.DeleteImageAsync(imageId, productItemId)
        });
}
