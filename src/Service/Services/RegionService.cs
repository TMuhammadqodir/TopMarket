using AutoMapper;
using Data.IRepositories;
using Domain.Configuration;
using Domain.Entities.Addresses;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Service.DTOs.Regions;
using Service.Exceptions;
using Service.Extensions;
using Service.Helpers;
using Service.Interfaces;

namespace Service.Services;

public class RegionService:IRegionService
{
    private readonly IMapper mapper;
    private readonly IRepository<Region> regionRepository;
    public RegionService(IRepository<Region> regionRepository, IMapper mapper)
    {
        this.regionRepository = regionRepository;
        this.mapper = mapper;
    }

    public async Task<bool> SetAsync(CancellationToken cancellationToken = default)
    {
        var dbSource = this.regionRepository.GetAll();
        if (dbSource.Any())
            throw new AlreadyExistException("Regions are already exist");

        string path = PathHepler.RegionPath;

        var source = File.ReadAllText(path);
        var regions = JsonConvert.DeserializeObject<IEnumerable<RegionCreationDto>>(source);

        foreach (var region in regions)
        {
            var mappedRegion = this.mapper.Map<Region>(region);
            await this.regionRepository.AddAsync(mappedRegion);
            await this.regionRepository.SaveAsync(cancellationToken);
        }
        return true;
    }

    public async Task<RegionResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var region = await this.regionRepository.GetAsync(rr => rr.Id.Equals(id), 
            includes: new[] { "Country" },
            cancellationToken:cancellationToken)
            ?? throw new NotFoundException("This region is not found");

        var mappedRegion = this.mapper.Map<RegionResultDto>(region);
        return mappedRegion;
    }

    public async Task<IEnumerable<RegionResultDto>> RetrieveAllAsync(PaginationParams @params, CancellationToken cancellationToken = default)
    {
        var regions = await this.regionRepository.GetAll(includes: new[] { "Country" })
            .ToPaginate(@params)
            .ToListAsync(cancellationToken);
        var result = this.mapper.Map<IEnumerable<RegionResultDto>>(regions);
        return result;
    }
}
