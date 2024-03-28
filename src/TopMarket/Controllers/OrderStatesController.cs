using Microsoft.AspNetCore.Mvc;
using Service.DTOs.OrderStates;
using Service.Interfaces;
using TopMarket.Models;

namespace TopMarket.Controllers;

public class OrderStatesController : BaseController
{
    private readonly IOrderStatusService orderStatusService;
    public OrderStatesController(IOrderStatusService orderStatusService)
    {
        this.orderStatusService = orderStatusService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> PostAsync(OrderStatusCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.orderStatusService.CreateAsync(dto)
        });


    [HttpPut("update")]
    public async Task<IActionResult> PutAsync(OrderStatusUpdateDto dto)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.orderStatusService.UpdateAsync(dto)
       });


    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.orderStatusService.DeleteAsync(id)
       });


    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.orderStatusService.GetByIdAsync(id)
       });


    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync()
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.orderStatusService.GetAllAsync()
       });
}
