using AutoMapper;
using Data.IRepositories;
using Domain.Entities.UserFolder;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.DTOs.Users;
using Service.Exceptions;
using Service.Helpers;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Services;


public class AuthService : IAuthService
{
    private readonly IMapper mapper;
    private readonly IConfiguration configuration;
    private readonly IRepository<User> repository;
    private readonly ICartService cartService;

    public AuthService(IRepository<User> repository, IMapper mapper, IConfiguration configuration, ICartService cartService)
    {
        this.repository = repository;
        this.mapper = mapper;
        this.configuration = configuration;
        this.cartService = cartService;
    }
    public async Task<UserResultDto> RegisterAsync(UserCreationDto dto)
    {
        User? user = await this.repository.GetAsync(x => x.Phone.Equals(dto.Phone));
        if (user is not null)
            throw new AlreadyExistException("This phone already exist");

        var mapped= mapper.Map<User>(dto);

        PasswordHash.Encrypt(dto.Password,out byte[] passwordhash,out byte[] salt);
        
        mapped.PasswordSalt= salt;
        mapped.PasswordHash= passwordhash;
        mapped.UserRole = UserRole.Customer;
        mapped.CartId = (await this.cartService.CreateAsync()).Id;

        await this.repository.AddAsync(mapped);
        await this.repository.SaveAsync();

        var result = this.mapper.Map<UserResultDto>(mapped);
        return result;
    }

    public async Task<string> LoginAsync(UserLoginDto dto)
    {
        User? user = await this.repository.GetAsync(x => x.Phone.Equals(dto.Phone))
            ?? throw new NotFoundException("This nomber not found");
        
        if (!PasswordHash.VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
            throw new Exception("Wrong password");

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                 new Claim("Phone", user.Phone),
                 new Claim("Id", user.Id.ToString()),
                 new Claim(ClaimTypes.Role, user.UserRole.ToString()),
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        string result = tokenHandler.WriteToken(token);
        return result;
    }

    public async Task<bool> ChangePasswordAsync(UserChangePassword dto)
    {
        User? user= await this.repository.GetAsync(x => x.Id.Equals(dto.UserId))
            ?? throw new NotFoundException("User not found");
        
        PasswordHash.Encrypt(dto.Password,out byte[] passwordhash, out byte[] salt);

        user.PasswordHash= passwordhash;
        user.PasswordSalt= salt;
        this.repository.Update(user);
        await this.repository.SaveAsync();

        return true;
    }

    public async Task<UserResultDto> UpdateAsync(UserUpdateDto dto)
    {
        User? user = await this.repository.GetAsync(dto.Id)
            ?? throw new NotFoundException("User not Found");

        var mapped = mapper.Map(dto,user);
        
        this.repository.Update(mapped);
        await this.repository.SaveAsync();
        
        var result = this.mapper.Map<UserResultDto>(mapped);
        return result;
    }
    public async Task<UserResultDto> GetByIdAsync(long id)
    {
        User? user = await this.repository.GetAsync(id)
            ?? throw new NotFoundException("User not found");

        var result = mapper.Map<UserResultDto>(user);
        return result;
    }

    public async Task<IEnumerable<UserResultDto>> GetAllAsync()
    {
        var users = await this.repository.GetAll().ToListAsync();

        var result = mapper.Map<IEnumerable<UserResultDto>>(users);
        return result;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        User? user = await  this.repository.GetAsync(id)
            ?? throw new NotFoundException("User not found");

        this.repository.Delete(user);
        await this.repository.SaveAsync();
        return true;
    }

    public async Task<bool> UserUpdateRole(long id, UserRole role)
    {
        User? user = await this.repository.GetAsync(id)
            ?? throw new NotFoundException("User not found");

        user.UserRole = role;
        await this.repository.SaveAsync();

        return true;
    }

    public async Task<bool> DestroyAsync(long id)
    {
        User? user = await repository.GetAsync(id)
            ?? throw new NotFoundException("User not found");

        this.repository.Destroy(user);
        await this.repository.SaveAsync() ;

        return true;
    }
}
