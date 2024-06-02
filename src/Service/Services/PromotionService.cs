using AutoMapper;
using Data.IRepositories;
using Domain.Entities.ProductFolder;
using Microsoft.EntityFrameworkCore;
using Service.DTOs.Categories;
using Service.DTOs.Promotions;
using Service.Exceptions;
using Service.Interfaces;

namespace Service.Services;

public class PromotionService : IPromotionService
{
    private readonly IMapper mapper;
    private readonly IRepository<Promotion> promationRepository;
    public PromotionService(IMapper mapper, IRepository<Promotion> promationRepository)
    {
        this.mapper = mapper;
        this.promationRepository = promationRepository;
    }

    public async Task<PromotionResultDto> CreateAsync(PromotionCreationDto dto, CancellationToken cancellationToken = default)
    {
        var existPromotion = await this.promationRepository.GetAsync(pr => pr.Name.ToLower().Equals(dto.Name.ToLower()) && pr.EndDate>=DateTime.UtcNow);
        if (existPromotion is not null)
            throw new AlreadyExistException($"This promotion already exist with {dto.Name}");

        var mappedPromotion = this.mapper.Map<Promotion>(dto);

        await this.promationRepository.AddAsync(mappedPromotion);
        await this.promationRepository.SaveAsync(cancellationToken);

        return this.mapper.Map<PromotionResultDto>(mappedPromotion);
    }

    public async Task<PromotionResultDto> UpdateAsync(PromotionUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var existPromotion = await this.promationRepository.GetAsync(pr => pr.Id.Equals(dto.Id), includes: new[] { "PromotionCategories" })
            ?? throw new NotFoundException($"This promotion was not found with {dto.Id}");

        if (!existPromotion.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase))
        {
            var existPromotion2 = await this.promationRepository.GetAsync(pr => pr.Name.ToLower().Equals(dto.Name.ToLower())&&pr.EndDate>=DateTime.UtcNow);
            if (existPromotion2 is not null)
                throw new AlreadyExistException($"This promotion already exist with {dto.Name}");
        }

        var mappedPromotion = this.mapper.Map(dto, existPromotion);

        this.promationRepository.Update(mappedPromotion);
        await this.promationRepository.SaveAsync(cancellationToken);

        return this.mapper.Map<PromotionResultDto>(mappedPromotion);
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var existPromotion = await this.promationRepository.GetAsync(pr => pr.Id.Equals(id))
            ?? throw new NotFoundException($"This promotion was not found with {id}");

        this.promationRepository.Delete(existPromotion);
        await this.promationRepository.SaveAsync(cancellationToken);

        return true;
    }

    public async Task<PromotionResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var existPromotion = await this.promationRepository.GetAsync(pr => pr.Id.Equals(id),
            includes: new[] { "PromotionCategories" }, 
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"This promotion was not found with {id}");

        return this.mapper.Map<PromotionResultDto>(existPromotion);
    }

    public async Task<IEnumerable<PromotionResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var promotions = await this.promationRepository.GetAll(includes: new[] { "PromotionCategories" })
            .ToListAsync( cancellationToken);

        return this.mapper.Map<IEnumerable<PromotionResultDto>>(promotions);
    }
}
