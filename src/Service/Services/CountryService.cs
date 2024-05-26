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
using Service.Validators.Countries;

namespace Service.Services;

public class CountryService:ICountryService
{

    private readonly IMapper mapper;
    private readonly IRepository<Country> countryRepository;
    private readonly CountryCreationValidator countryCreationValidator;
    public CountryService(IMapper mapper, IRepository<Country> countryRepository)
    {
        this.mapper = mapper;
        this.countryRepository = countryRepository;
        this.countryCreationValidator = new CountryCreationValidator();
    }

    public async Task<bool> SetAsync(CancellationToken cancellationToken = default)
    {
        var dbSource = this.countryRepository.GetAll();
        if (dbSource.Any())
            throw new AlreadyExistException("Countries are already exist");

        string path = PathHepler.CountryPath;
        var source = File.ReadAllText(path);
        var countries = JsonConvert.DeserializeObject<IEnumerable<CountryCreationDto>>(source);

        foreach (var country in countries)
        {
            var resultValidator = this.countryCreationValidator.Validate(country);
            if (resultValidator.Errors.Any())
                throw new CustomException(403, resultValidator.Errors.FirstOrDefault().ToString());

            var mappedCountry = this.mapper.Map<Country>(country);
            await this.countryRepository.AddAsync(mappedCountry);
            await this.countryRepository.SaveAsync(cancellationToken);
        }
        return true;
    }

    public async Task<CountryResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var country = await this.countryRepository.GetAsync(cr => cr.Id.Equals(id), cancellationToken: cancellationToken)
            ?? throw new NotFoundException("This country is not found");

        var mappedCountry = this.mapper.Map<CountryResultDto>(country);
        return mappedCountry;
    }

    public async Task<IEnumerable<CountryResultDto>> RetrieveAllAsync(PaginationParams @params, 
        CancellationToken cancellationToken = default)
    {
        var countries = await this.countryRepository.GetAll()
            .ToPaginate(@params)
            .ToListAsync(cancellationToken);

        var result = this.mapper.Map<IEnumerable<CountryResultDto>>(countries);
        return result;
    }
}
