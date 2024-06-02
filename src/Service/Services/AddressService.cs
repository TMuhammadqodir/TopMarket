using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Data.IRepositories;
using Domain.Configuration;
using Domain.Entities.Addresses;
using Service.DTOs.Addresses;
using Service.Exceptions;
using Service.Extensions;
using Service.Interfaces;
using Service.Validators.Addresses;
using FluentValidation;

namespace Service.Services;

public class AddressService : IAddressService
{
    private readonly ILogger<AddressService> logger;
    private readonly IMapper mapper;
    private readonly IRepository<Address> repository;
    private readonly AddressCreationValidator creationDtoValidator;
    private readonly AddressUpdateValidator updateDtoValidator;

    public AddressService(ILogger<AddressService> logger, IMapper mapper, IRepository<Address> repository)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.repository = repository;
        this.creationDtoValidator = new AddressCreationValidator();
        this.updateDtoValidator = new AddressUpdateValidator();
    }

    public async Task<AddressResultDto> CreateAsync(AddressCreationDto dto, CancellationToken cancellationToken = default)
    {
        await this.creationDtoValidator.ValidateAndThrowAsync(dto, cancellationToken);

        try
        {
            var newAddress = this.mapper.Map<Address>(dto);
            await this.repository.AddAsync(newAddress, cancellationToken);
            await this.repository.SaveAsync(cancellationToken);
            this.logger.LogInformation("New address has been successfully added.");
            
            return this.mapper.Map<AddressResultDto>(newAddress);
        }
        catch(Exception ex)
        {
            this.logger.LogError(ex, "An error occured while adding new address to the database.");
            throw;
        }
    }

    public async Task<AddressResultDto> ModifyAsync(AddressUpdateDto dto, CancellationToken cancellationToken = default)
    {
        await this.updateDtoValidator.ValidateAndThrowAsync(dto, cancellationToken);

        var address = await this.repository.GetAsync(dto.Id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Address with id = '{dto.Id}' is not found.");

        try
        {
            this.mapper.Map(dto, address);
            this.repository.Update(address);
            await this.repository.SaveAsync(cancellationToken);
            this.logger.LogInformation("Address # {id} has been succesfully updated.", dto.Id);

            return mapper.Map<AddressResultDto>(address);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "An error occured while updating the address # {id}", dto.Id);
            throw;
        }
    }

    public async Task<bool> RemoveAsync(long id, bool destroy = false, CancellationToken cancellationToken = default)
    {
        var address = await this.repository.GetAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Address with id = '{id}' is not found.");

        try
        {
            if (destroy)
                this.repository.Destroy(address);
            else
                this.repository.Delete(address);
            
            await this.repository.SaveAsync(cancellationToken);
            this.logger.LogInformation("Address # {id} has been successfully {action}.", id, destroy ? "destroyed" : "deleted");

            return true;
        }
        catch(Exception ex)
        {
            this.logger.LogError(ex, "An error occured while deleting the address # {id}.", id);
            return false;
        }
    }

    public async Task<IEnumerable<AddressResultDto>> RetrieveAllAsync(CancellationToken cancellationToken = default)
    {
        var inclusion = new[] { "Country", "Region", "District" };
        var addresses = await this.repository
                            .GetAll(includes: inclusion)
                            .ToListAsync(cancellationToken);

        return this.mapper.Map<IEnumerable<AddressResultDto>>(addresses);
    }

    public async Task<IEnumerable<AddressResultDto>> RetrieveAllAsync(PaginationParams @params, CancellationToken cancellationToken = default)
    {
        var addresses = await this.repository.GetAll(includes: new[] { "Country", "Region", "District" })
            .ToPaginate(@params)
            .ToListAsync();

        return this.mapper.Map<IEnumerable<AddressResultDto>>(addresses);
    }

    public async Task<AddressResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var inclusion = new[] { "Country", "Region", "District" };
        var address = await this.repository.GetAsync(id, inclusion, cancellationToken)
            ?? throw new NotFoundException($"Address with id = '{id}' is not found.");

        return this.mapper.Map<AddressResultDto>(address);
    }
}
