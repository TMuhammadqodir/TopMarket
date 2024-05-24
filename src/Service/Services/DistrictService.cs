using AutoMapper;
using Data.IRepositories;
using Domain.Configuration;
using Domain.Entities.Addresses;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Service.DTOs.Districts;
using Service.Exceptions;
using Service.Extensions;
using Service.Helpers;
using Service.Interfaces;
using Service.Validators.Districts;

namespace Service.Services;

public class DistrictService:IDistrictService
{
    private readonly IMapper mapper;
    private readonly IRepository<District> repository;
    private readonly DistrictCreationValidator districtCreationValidator;
    public DistrictService(IMapper mapper, IRepository<District> repository)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.districtCreationValidator = new DistrictCreationValidator();
    }

    public async Task<bool> SetAsync()
    {
        var dbSource = this.repository.GetAll();
        if (dbSource.Any())
            throw new AlreadyExistException("Districts are already exist");

        string path = PathHepler.DistrictPath;
        var source = File.ReadAllText(path);
        var districts = JsonConvert.DeserializeObject<IEnumerable<DistrictCreationDto>>(source);

        foreach (var district in districts)
        {
            var reusltDistrictCreationValidator = this.districtCreationValidator.Validate(district);
            if (reusltDistrictCreationValidator.Errors.Any())
                throw new CustomException(403, reusltDistrictCreationValidator.Errors.FirstOrDefault().ToString());

            var mappedDistrict = this.mapper.Map<District>(district);
            await this.repository.AddAsync(mappedDistrict);
            await this.repository.SaveAsync();
        }
        return true;
    }

    public async Task<DistrictResultDto> RetrieveByIdAsync(long id)
    {
        var district = await this.repository.GetAsync(r => r.Id.Equals(id), includes: new[] { "Region.Country" })
            ?? throw new NotFoundException("This district is not found");

        var mappedDistrict = this.mapper.Map<DistrictResultDto>(district);
        return mappedDistrict;
    }

    public async Task<IEnumerable<DistrictResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var districts = await this.repository.GetAll(includes: new[] { "Region.Country" })
            .ToPaginate(@params)
            .ToListAsync();
        var result = this.mapper.Map<IEnumerable<DistrictResultDto>>(districts);
        return result;
    }
}
