using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Categories;
using Service.Interfaces;
using TopMarket.Models;

namespace TopMarket.Controllers;

public class CategoriesController : BaseController
{
    private readonly ICategoryService categoryService;
    public CategoriesController(ICategoryService categoryService)
    {
        this.categoryService = categoryService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> PostAsync(CategoryCreationDto dto)
    {
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.categoryService.CreateAsync(dto)
        });
    }


    [HttpPut("update")]
    public async Task<IActionResult> PutAsync(CategoryUpdateDto dto)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.categoryService.ModifyAsync(dto)
       });


    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.categoryService.RemoveAsync(id)
       });


    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.categoryService.RetrieveAsync(id)
       });


    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync()
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.categoryService.RetrieveAllAsync()
       });
}
