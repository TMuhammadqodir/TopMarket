using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Attachments;
using Service.DTOs.Products;
using Service.Interfaces;
using TopMarket.Models;

namespace TopMarket.Controllers;

public class ProductsController : BaseController
{
    private readonly IProductService productService;
    public ProductsController(IProductService productService)
    {
        this.productService = productService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> PostAsync(ProductCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.productService.CreateAsync(dto)
        });


    [HttpPut("update")]
    public async Task<IActionResult> PutAsync(ProductUpdateDto dto)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.productService.ModifyAsync(dto)
       });


    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.productService.RemoveAsync(id)
       });


    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.productService.RetrieveAsync(id)
       });


    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync()
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.productService.RetrieveAllAsync()
       });


    [HttpPost("image-upload")]
    public async Task<IActionResult> UploadImageAsync(long productId, [FromForm] AttachmentCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.productService.UploadImageAsync(productId, dto)
        });
}
