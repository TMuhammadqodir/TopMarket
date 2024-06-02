using AutoMapper;
using Data.IRepositories;
using Domain.Entities.ProductFolder;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.DTOs.Categories;
using Service.Exceptions;
using Service.Interfaces;
using Service.Validators.Categories;

namespace Service.Services;

public class CategoryService : ICategoryService
{
    private readonly ILogger<CategoryService> logger;
    private readonly IMapper mapper;
    private readonly IRepository<Category> repository;
    private readonly CategoryCreationValidator creationDtoValidator;
    private readonly CategoryUpdateValidator updateDtoValidator;

    public CategoryService(ILogger<CategoryService> logger, IMapper mapper, IRepository<Category> repository)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.repository = repository;
        this.creationDtoValidator = new CategoryCreationValidator();
        this.updateDtoValidator = new CategoryUpdateValidator();
    }

    public async Task<CategoryResultDto> CreateAsync(CategoryCreationDto dto, CancellationToken cancellationToken = default)
    {
        await this.creationDtoValidator.ValidateAndThrowAsync(dto, cancellationToken);

        try
        {
            var newCategory = this.mapper.Map<Category>(dto);
            await this.repository.AddAsync(newCategory, cancellationToken);
            await this.repository.SaveAsync(cancellationToken);
            this.logger.LogInformation("New category '{category}' has been successfully created.", dto.Name);

            return this.mapper.Map<CategoryResultDto>(newCategory);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "An error occured while creating category '{name}'", dto.Name);
            throw;
        }
    }

    public async Task<CategoryResultDto> ModifyAsync(CategoryUpdateDto dto, CancellationToken cancellationToken = default)
    {
        await this.updateDtoValidator.ValidateAndThrowAsync(dto, cancellationToken);

        var inclusion = new[] { "Products", "Variations", "PromotionCategories" };
        var category = await this.repository.GetAsync(dto.Id, inclusion, cancellationToken)
            ?? throw new NotFoundException($"Category with id # {dto.Id} is not found.");

        try
        {
            this.mapper.Map(dto, category);
            this.repository.Update(category);
            await this.repository.SaveAsync(cancellationToken);
            this.logger.LogInformation("Category has been successfully updated.");

            return this.mapper.Map<CategoryResultDto>(category);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "An error occured while updating the category {name}", category.Name);
            throw;
        }
    }

    public async Task<bool> RemoveAsync(long id, bool destroy = false, CancellationToken cancellationToken = default)
    {
        var category = await this.repository.GetAsync(id, cancellationToken: cancellationToken) 
            ?? throw new NotFoundException($"Category with id # {id} is not found.");

        try
        {
            if (destroy)
                this.repository.Destroy(category);
            else
                this.repository.Delete(category);
            
            await this.repository.SaveAsync(cancellationToken);
            this.logger.LogInformation("Category '{name}' has been successfully {action}.", category.Name, destroy ? "destroyed" : "deleted");
            
            return true;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "An error occured while deleting category {name}", category.Name);
            return false;
        }
    }

    public async Task<CategoryResultDto> RetrieveAsync(long id, CancellationToken cancellationToken = default)
    {
        var inclusion = new[] { "Products", "Variations", "PromotionCategories" };
        var category = await this.repository.GetAsync(id, inclusion, cancellationToken)
            ?? throw new NotFoundException($"Category with id # {id} is not found.");

        return this.mapper.Map<CategoryResultDto>(category);
    }

    public async Task<IEnumerable<CategoryResultDto>> RetrieveAllAsync(CancellationToken cancellationToken = default)
    {
        var inclusion = new[] { "Products", "Variations", "PromotionCategories" };
        var categories = await this.repository
            .GetAll(includes: inclusion)
            .ToListAsync(cancellationToken);

        return this.mapper.Map<IEnumerable<CategoryResultDto>>(categories);
    }
}
