using Domain.Enums;
using Service.DTOs.Users;
using System.Globalization;

namespace Service.Interfaces;

public interface IAuthService
{
    Task<UserResultDto> RegisterAsync(UserCreationDto dto,CancellationToken cancellationToken = default);
    Task<string> LoginAsync(UserLoginDto dto,CancellationToken cancellationToken = default);
    Task<bool> ChangePasswordAsync(UserChangePassword dto, CancellationToken cancellationToken = default);
    Task<UserResultDto> UpdateAsync(UserUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id,CancellationToken cancellationToken = default);
    Task<bool> DestroyAsync(long id, CancellationToken cancellationToken = default);
    Task<UserResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UserUpdateRole(long id, EUserRole role,CancellationToken cancellationToken = default);
}
