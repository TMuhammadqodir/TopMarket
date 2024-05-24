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
    private readonly IRepository<User> userRepository;
    private readonly ICartService cartService;

    public AuthService(IRepository<User> userRepository,
                       IMapper mapper, 
                       IConfiguration configuration, 
                       ICartService cartService)
    {
        this.userRepository = userRepository;
        this.mapper = mapper;
        this.configuration = configuration;
        this.cartService = cartService;
    }
    public async Task<UserResultDto> RegisterAsync(UserCreationDto dto,CancellationToken cancellationToken=default)
    {
        User? user = await this.userRepository.GetAsync(ur => ur.Phone.Equals(dto.Phone));
        if (user is not null)
            throw new AlreadyExistException("This phone already exist");

        var mapped= mapper.Map<User>(dto);

        PasswordHash.Encrypt(dto.Password,out byte[] passwordhash,out byte[] salt);
        
        mapped.PasswordSalt= salt;
        mapped.PasswordHash= passwordhash;
        mapped.UserRole = EUserRole.Customer;
        mapped.CartId = (await this.cartService.CreateAsync()).Id;

        await this.userRepository.AddAsync(mapped,cancellationToken);
        await this.userRepository.SaveAsync(cancellationToken);

        var result = this.mapper.Map<UserResultDto>(mapped);
        return result;
    }

    public async Task<string> LoginAsync(UserLoginDto dto, CancellationToken cancellationToken = default)
    {
        User? user = await this.userRepository.GetAsync(ur => ur.Phone.Equals(dto.Phone))
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

    public async Task<bool> ChangePasswordAsync(UserChangePassword dto, CancellationToken cancellationToken = default)
    {
        User? user= await this.userRepository.GetAsync(ur => ur.Id.Equals(dto.UserId))
            ?? throw new NotFoundException("User not found");
        
        PasswordHash.Encrypt(dto.Password,out byte[] passwordhash, out byte[] salt);

        user.PasswordHash= passwordhash;
        user.PasswordSalt= salt;
        this.userRepository.Update(user);
        await this.userRepository.SaveAsync(cancellationToken);

        return true;
    }

    public async Task<UserResultDto> UpdateAsync(UserUpdateDto dto, CancellationToken cancellationToken = default)
    {
        User? user = await this.userRepository.GetAsync(dto.Id)
            ?? throw new NotFoundException("User not Found");

        var mapped = mapper.Map(dto,user);
        
        this.userRepository.Update(mapped);
        await this.userRepository.SaveAsync(cancellationToken);
        
        var result = this.mapper.Map<UserResultDto>(mapped);
        return result;
    }
    public async Task<UserResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        User? user = await this.userRepository.GetAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException("User not found");

        var result = mapper.Map<UserResultDto>(user);
        return result;
    }

    public async Task<IEnumerable<UserResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await this.userRepository.GetAll().ToListAsync(cancellationToken:cancellationToken);

        var result = mapper.Map<IEnumerable<UserResultDto>>(users);
        return result;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken= default)
    {
        User? user = await  this.userRepository.GetAsync(id)
            ?? throw new NotFoundException("User not found");

        this.userRepository.Delete(user);
        await this.userRepository.SaveAsync(cancellationToken);
        return true;
    }

    public async Task<bool> UserUpdateRole(long id, EUserRole role, CancellationToken cancellationToken= default)
    {
        User? user = await this.userRepository.GetAsync(id)
            ?? throw new NotFoundException("User not found");

        user.UserRole = role;
        await this.userRepository.SaveAsync();

        return true;
    }

    public async Task<bool> DestroyAsync(long id, CancellationToken cancellationToken= default)
    {
        User? user = await userRepository.GetAsync(id)
            ?? throw new NotFoundException("User not found");

        this.userRepository.Destroy(user);
        await this.userRepository.SaveAsync(cancellationToken) ;

        return true;
    }
}
