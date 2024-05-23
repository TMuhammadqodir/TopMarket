using AutoMapper;
using Data.IRepositories;
using Domain.Configuration;
using Domain.Entities.Addresses;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Service.DTOs.Countries;
using Service.Exceptions;
using Service.Extensions;
using Service.Helpers;
using Service.Interfaces;

namespace Service.Services;

public class CountryService:ICountryService
{

    private readonly IMapper mapper;
    private readonly IRepository<Country> repository;
    public CountryService(IMapper mapper, IRepository<Country> repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    public async Task<bool> SetAsync(CancellationToken cancellationToken = default)
    {
        var dbSource = this.repository.GetAll();
        if (dbSource.Any())
            throw new AlreadyExistException("Countries are already exist");

        string path = PathHepler.CountryPath;
        var source = File.ReadAllText(path);
        var countries = JsonConvert.DeserializeObject<IEnumerable<CountryCreationDto>>(source);

        foreach (var country in countries)
        {
            var mappedCountry = this.mapper.Map<Country>(country);
            await this.repository.AddAsync(mappedCountry);
            await this.repository.SaveAsync(cancellationToken);
        }
        return true;
    }

    public async Task<CountryResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var country = await this.repository.GetAsync(r => r.Id.Equals(id), cancellationToken: cancellationToken)
            ?? throw new NotFoundException("This country is not found");

        var mappedCountry = this.mapper.Map<CountryResultDto>(country);
        return mappedCountry;
    }

    public async Task<IEnumerable<CountryResultDto>> RetrieveAllAsync(PaginationParams @params, 
        CancellationToken cancellationToken = default)
    {
        var countries = await this.repository.GetAll()
            .ToPaginate(@params)
            .ToListAsync(cancellationToken);

        var result = this.mapper.Map<IEnumerable<CountryResultDto>>(countries);
        return result;
    }
}
