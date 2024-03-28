using Domain.Enums;
using Service.DTOs.Users;
using System.Globalization;

namespace Service.Interfaces;

public interface IAuthService
{
    Task<UserResultDto> RegisterAsync(UserCreationDto dto);
    Task<string> LoginAsync(UserLoginDto dto);
    Task<bool> ChangePasswordAsync(UserChangePassword dto);
    Task<UserResultDto> UpdateAsync(UserUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<bool> DestroyAsync(long id);
    Task<UserResultDto> GetByIdAsync(long id);
    Task<IEnumerable<UserResultDto>> GetAllAsync();
    Task<bool> UserUpdateRole(long id, UserRole role);
}
