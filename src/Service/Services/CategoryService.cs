using AutoMapper;
using Data.IRepositories;
using Domain.Entities.ProductFolder;
using Microsoft.EntityFrameworkCore;
using Service.DTOs.Categories;
using Service.Exceptions;
using Service.Interfaces;

namespace Service.Services;

public class CategoryService :ICategoryService
{
    private readonly IMapper mapper;
    private readonly IRepository<Category> repository;
    public CategoryService(IMapper mapper, IRepository<Category> repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    public async Task<CategoryResultDto> CreateAsync(CategoryCreationDto dto)
    {
        if (dto.ParentId != 0)
        {
            var existParentCategory = await this.repository.GetAsync(c => c.Id.Equals(dto.ParentId))
                ?? throw new NotFoundException($"This parent category was not found with {dto.ParentId}");
        }

        var existCategory = await this.repository.GetAsync(c => c.Name.ToLower().Equals(dto.Name.ToLower()));
        if (existCategory is not null)
            throw new AlreadyExistException($"This category already exist with {dto.Name}");

        var mappedCategory = this.mapper.Map<Category>(dto);

        await this.repository.AddAsync(mappedCategory);
        await this.repository.SaveAsync();

        return this.mapper.Map<CategoryResultDto>(mappedCategory);
    }

    public async Task<CategoryResultDto> UpdateAsync(CategoryUpdateDto dto)
    {
        var existCategory = await this.repository.GetAsync(c => c.Id.Equals(dto.Id), includes: new[] { "Products", "Variations", "PromotionCategories" })
            ?? throw new NotFoundException($"This category was not found with {dto.Id}");

        if (dto.ParentId != 0)
        {
            var existParentCategory = await this.repository.GetAsync(c => c.Id.Equals(dto.ParentId))
                ?? throw new NotFoundException($"This parent category was not found with {dto.ParentId}");
        }

        if(!existCategory.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase))
        {
            var existCategoryName = await this.repository.GetAsync(c => c.Name.ToLower().Equals(dto.Name.ToLower()));
            if (existCategoryName is not null)
                throw new AlreadyExistException($"This category already exist with {dto.Name}");
        }

        var mappedCategory = this.mapper.Map(dto, existCategory);

        this.repository.Update(mappedCategory);
        await this.repository.SaveAsync();

        return this.mapper.Map<CategoryResultDto>(mappedCategory);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existCategory = await this.repository.GetAsync(c => c.Id.Equals(id)) 
            ?? throw new NotFoundException($"This category was not found with {id}");

        this.repository.Delete(existCategory);
        await this.repository.SaveAsync();

        return true;
    }

    public async Task<CategoryResultDto> GetByIdAsync(long id)
    {
        var existCategory = await this.repository.GetAsync(c => c.Id.Equals(id), includes: new[] { "Products", "Variations", "PromotionCategories" })
            ?? throw new NotFoundException($"This category was not found with {id}");

        return this.mapper.Map<CategoryResultDto>(existCategory);
    }

    public async Task<IEnumerable<CategoryResultDto>> GetAllAsync()
    {
        var categories = await this.repository.GetAll(includes: new[] { "Products", "Variations", "PromotionCategories" }).ToListAsync();

        return this.mapper.Map<IEnumerable<CategoryResultDto>>(categories);
    }
}
