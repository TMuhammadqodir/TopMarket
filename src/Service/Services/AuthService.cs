﻿using AutoMapper;
using Service.Helpers;
using Service.Interfaces;
using Data.IRepositories;
using Service.DTOs.Users;
using Service.Exceptions;
using Domain.Entities.UserFolder;
using Microsoft.Extensions.Configuration;
using Domain.Enums;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace Service.Services;


public class AuthService : IAuthsService
{
    private readonly IMapper mapper;
    private readonly IConfiguration configuration;
    private readonly IRepository<User> repository;

    public AuthService(IRepository<User> repository, IMapper mapper, IConfiguration configuration)
    {
        this.repository = repository;
        this.mapper = mapper;
        this.configuration = configuration;
    }
    public async Task<UserResultDto> RegisterAsync(UserCreationDto dto)
    {
        User user= await repository.GetAsync(x=>x.Phone.Equals(dto.Phone));
        if (user is not null)
            throw new AlreadyExistException("This phone already exist");
        var mapped= mapper.Map<User>(dto);

        PasswordHash.Encrypt(dto.Password,out byte[] passwordhash,out byte[] salt);
        mapped.PasswordSalt= salt;
        mapped.PasswordHash= passwordhash;
        mapped.UserRole = Domain.Enums.UserRole.Customer;

        await repository.AddAsync(mapped);
        await repository.SaveAsync();

        var result = mapper.Map<UserResultDto>(mapped);

        return result;

    }
    public async Task<string> LoginAsync(UserLoginDto dto)
    {
        User user = await repository.GetAsync(x => x.Phone.Equals(dto.Phone));
        if (user is null)
            throw new NotFoundException("This nomber not found");
        else if (!PasswordHash.VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
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
        User user= await repository.GetAsync(x=>x.Id.Equals(dto.UserId));
        if (user is null)
            throw new NotFoundException("User not found");
        PasswordHash.Encrypt(dto.Password,out byte[] passwordhash, out byte[] salt);

        user.PasswordHash= passwordhash;
        user.PasswordSalt= salt;
        repository.Update(user);
        await repository.SaveAsync();

        return true;
    }

    public async Task<UserResultDto> UpdateAsync(UserUpdateDto dto)
    {
        User user= await repository.GetAsync(x=>x.Id.Equals(dto.Id));
        if (user is null)
            throw new NotFoundException("User not Found");

         user.UpdatetAt= DateTime.UtcNow;
        var mapped= mapper.Map(dto,user);
        repository.Update(mapped);
        await repository.SaveAsync();
        var result= mapper.Map<UserResultDto>(mapped);
        return result;
    }
    public async Task<UserResultDto> GetByIdAsync(long id)
    {
        User user = await repository.GetAsync(x => x.Id.Equals(id));
        if (user is null)
            throw new NotFoundException("User not found");

        var mapped = mapper.Map<UserResultDto>(user);
        return mapped;
    }

    public async Task<IEnumerable<UserResultDto>> GetAllAsync()
    {
        var users = repository.GetAll().ToList();

        var mapped = mapper.Map<IEnumerable<UserResultDto>>(users);

        return mapped;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        User user = await repository.GetAsync(x => x.Id.Equals(id));
        if (user is null)
            throw new NotFoundException("User not found");

        repository.Delete(user);
        await repository.SaveAsync();
        return true;
    }

    public async Task<bool> UserUpdateRole(long id, UserRole role)
    {
        User user = await repository.GetAsync(x => x.Id.Equals(id));
        if (user is null)
            throw new NotFoundException("User not found");

        user.UserRole = role;
        await repository.SaveAsync();
        return true;
    }
}