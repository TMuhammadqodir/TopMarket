
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Users;
using Service.Interfaces;
using System.Security.Claims;
using TopMarket.Models;

namespace TopMarket.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuthService authsService;
        public UserController(IAuthService authsService)
        {
            this.authsService = authsService;
        }
        [HttpPost("create")]
        public async ValueTask<IActionResult> PostAsync(UserCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await authsService.RegisterAsync(dto)
        });

        [HttpPost("login")]
        public async ValueTask<IActionResult> LoginAsync(UserLoginDto dto)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Succes",
                Data = await authsService.LoginAsync(dto)
            });

        [Authorize(Roles = "SuperAdmin,Admin,Customer")]
        [HttpPut("update")]

        public async ValueTask<IActionResult> UpdateAsync(UserViewDto dto)
        {
            long id = Convert.ToInt32(HttpContext.User.FindFirstValue("id"));
            var role = Convert.ToInt32(HttpContext.User.FindFirstValue("Role"));
            UserRole user = (UserRole)role;

            var update = new UserUpdateDto()
            {
                Id = id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                UserRole = user,
            };

            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Succes",
                Data = await authsService.UpdateAsync(update)
            });
        }

        [Authorize(Roles = "SuperAdmin,Admin,Customer")]
        [HttpPut("changepassword")]

        public async Task<IActionResult> ChangePasswordAsycn(UserPasswordView dto)
        {
            long id = Convert.ToInt32(HttpContext.User.FindFirstValue("id"));

            var user = new UserChangePassword
            {
                UserId = id,
                Password = dto.Password,
                ConfirmPassword = dto.ConfirmPassword
            };

            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Succes",
                Data = await authsService.ChangePasswordAsync(user)
            });
        }

        [Authorize(Roles = "SuperAdmin,Admin,Customer")]
        [HttpDelete("delete")]
        
        public async Task<IActionResult> DeleteAsync()
        {
            long id = Convert.ToInt32(HttpContext.User.FindFirstValue("id"));

            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Succes",
                Data = await authsService.DeleteAsync(id)
            });
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("destroyby-id")]

        public async Task<IActionResult> DestroyAsync(long id)
        {
            long Id = Convert.ToInt32(HttpContext.User.FindFirstValue("id"));

            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Succes",
                Data = await authsService.DestroyAsync(Id)
            });
        }


        [Authorize(Roles = "SuperAdmin,Admin,Customer")]
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetByIdAsync()
        {
            long id = long.Parse(HttpContext.User.FindFirstValue("id"));
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Succes",
                Data = await authsService.GetByIdAsync(id)
            });
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("get-id")]
        public async Task<IActionResult> GetIdForAdminAsync(long id)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Succes",
                Data = await authsService.GetByIdAsync(id)
            });


        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Succes",
                Data = await authsService.GetAllAsync()
            });
        }


        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("update-role")]

        public async Task<IActionResult> UpdateUserRole(long id, UserRole role)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Succes",
                Data = await authsService.UserUpdateRole(id, role)
            });

    }

}
