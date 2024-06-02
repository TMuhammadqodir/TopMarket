using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Data.IRepositories;
using Domain.Entities.ProductFolder;
using Service.DTOs.PromotionCategories;
using Service.Exceptions;
using Service.Interfaces;
using Service.Validators.PromotionCategories;
using FluentValidation;

namespace Service.Services;

public class PromotionCategoryService : IPromotionCategoryService
{
    private readonly ILogger<PromotionCategoryService> logger;
    private readonly IMapper mapper;
    private readonly IRepository<PromotionCategory> repository;
    private readonly PromotionCategoryCreationValidator creationDtoValidator;
    private readonly PromotionCategoryUpdateValidator updateDtoValidator;

    public PromotionCategoryService(ILogger<PromotionCategoryService> logger, IMapper mapper, IRepository<PromotionCategory> repository)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.repository = repository;
        this.creationDtoValidator = new PromotionCategoryCreationValidator();
        this.updateDtoValidator = new PromotionCategoryUpdateValidator();
    }
    
    public async Task<PromotionCategoryResultDto> CreateAsync(PromotionCategoryCreationDto dto, CancellationToken cancellationToken = default)
    {
        await this.creationDtoValidator.ValidateAndThrowAsync(dto, cancellationToken);

        try
        {
            var newPromotionCategory = this.mapper.Map<PromotionCategory>(dto);
            await this.repository.AddAsync(newPromotionCategory, cancellationToken);
            await this.repository.SaveAsync(cancellationToken);
            this.logger.LogInformation("A new promotion category has been successfully created.");

            return this.mapper.Map<PromotionCategoryResultDto>(newPromotionCategory);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "An error occured while creating new promotion category.");
            throw;
        }
    }

    public async Task<PromotionCategoryResultDto> ModifyAsync(PromotionCategoryUpdateDto dto, CancellationToken cancellationToken = default)
    {
        await this.updateDtoValidator.ValidateAndThrowAsync(dto, cancellationToken);

        var promotionCategory = await this.repository.GetAsync(dto.Id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Promotion category with id # '{dto.Id}' is not found.");

        try
        {
            this.mapper.Map(dto, promotionCategory);
            this.repository.Update(promotionCategory);
            await this.repository.SaveAsync(cancellationToken);
            this.logger.LogInformation("Promotion category # {id} has been successfully updated.", promotionCategory.Id);

            return this.mapper.Map<PromotionCategoryResultDto>(promotionCategory);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "An error occured while updating promotion category # {id}", promotionCategory.Id);
            throw;
        }
    }

    public async Task<bool> RemoveAsync(long id, bool destroy = false, CancellationToken cancellationToken = default)
    {
        var promotionCategory = await this.repository.GetAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Promotion category with id # '{id}' is not found.");

        try
        {
            if (destroy)
                this.repository.Destroy(promotionCategory);
            else
                this.repository.Delete(promotionCategory);

            await this.repository.SaveAsync(cancellationToken);
            this.logger.LogInformation("Promotion category # '{id}' has been successfully {action}.", id, destroy ? "destroyed" : "deleted");

            return true;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "An error occured while deleting promotion category # '{id}'", id);
            return false;
        }
    }

    public async Task<IEnumerable<PromotionCategoryResultDto>> RetrieveAllAsync(CancellationToken cancellationToken = default)
    {
        var promotionCategories = await this.repository
            .GetAll()
            .ToListAsync(cancellationToken);

        return this.mapper.Map<IEnumerable<PromotionCategoryResultDto>>(promotionCategories);
    }

    public async Task<PromotionCategoryResultDto> RetrieveAsync(long id, CancellationToken cancellationToken = default)
    {
        var promotionCategory = await this.repository.GetAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Promotion category with id # '{id}' is not found.");
        
        return this.mapper.Map<PromotionCategoryResultDto>(promotionCategory);
    }
}
