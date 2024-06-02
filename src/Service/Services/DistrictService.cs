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
    private readonly IRepository<District> districtRepository;
    private readonly DistrictCreationValidator districtCreationValidator;
    public DistrictService(IMapper mapper, IRepository<District> districtRepository)
    {
        this.mapper = mapper;
        this.districtRepository = districtRepository;
        this.districtCreationValidator = new DistrictCreationValidator();
    }

    public async Task<bool> SetAsync(CancellationToken cancellationToken = default)
    {
        var dbSource = this.districtRepository.GetAll();
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
            await this.districtRepository.AddAsync(mappedDistrict);
            await this.districtRepository.SaveAsync(cancellationToken);
        }
        return true;
    }

    public async Task<DistrictResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var district = await this.districtRepository.GetAsync(dr => dr.Id.Equals(id), 
            includes: new[] { "Region.Country" }, 
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException("This district is not found");

        var mappedDistrict = this.mapper.Map<DistrictResultDto>(district);
        return mappedDistrict;
    }

    public async Task<IEnumerable<DistrictResultDto>> RetrieveAllAsync(PaginationParams @params, CancellationToken cancellationToken = default)
    {
        var districts = await this.districtRepository.GetAll(includes: new[] { "Region.Country" })
            .ToPaginate(@params)
            .ToListAsync(cancellationToken);
        var result = this.mapper.Map<IEnumerable<DistrictResultDto>>(districts);
        return result;
    }
}
