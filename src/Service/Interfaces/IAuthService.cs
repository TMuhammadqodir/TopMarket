using Domain.Enums;
using Service.DTOs.Users;
using System.Globalization;

namespace Service.Interfaces;

public interface IAuthService
{
    Task<UserResultDto> RegisterAsync(UserCreationDto dto,CancellationToken cancellationToken);
    Task<string> LoginAsync(UserLoginDto dto,CancellationToken cancellationToken);
    Task<bool> ChangePasswordAsync(UserChangePassword dto, CancellationToken cancellationToken);
    Task<UserResultDto> UpdateAsync(UserUpdateDto dto, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(long id,CancellationToken cancellationToken);
    Task<bool> DestroyAsync(long id, CancellationToken cancellationToken);
    Task<UserResultDto> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<IEnumerable<UserResultDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> UserUpdateRole(long id, EUserRole role,CancellationToken cancellationToken);
}
