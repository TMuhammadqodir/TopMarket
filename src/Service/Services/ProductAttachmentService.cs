using AutoMapper;
using Data.IRepositories;
using Domain.Entities.AttachmentFolder;
using Domain.Entities.ProductFolder;
using Microsoft.EntityFrameworkCore;
using Service.DTOs.ProductAttachments;
using Service.Exceptions;
using Service.Interfaces;

namespace Service.Services;

public class ProductAttachmentService : IProductAttachmentService
{
    private readonly IMapper mapper;
    private readonly IRepository<Product> productRepository;
    private readonly IRepository<ProductAttachment> repository;
    private readonly IRepository<Attachment> attachmentRepository;
    public ProductAttachmentService(
        IMapper mapper,
        IRepository<Product> productRepository,
        IRepository<ProductAttachment> repository,
        IRepository<Attachment> attachmentRepository)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.productRepository = productRepository;
        this.attachmentRepository = attachmentRepository;
    }

    public async Task<ProductAttachmentResultDto> CreateAsync(ProductAttachmentCreationDto dto, CancellationToken cancellationToken)
    {
        var existProduct = await this.productRepository.GetAsync(p => p.Id.Equals(dto.ProductId))
            ?? throw new NotFoundException($"This product was not found with {dto.ProductId}");

        var existAttachment = await this.attachmentRepository.GetAsync(p => p.Id.Equals(dto.AttachmentId))
            ?? throw new NotFoundException($"This attachment was not found with {dto.AttachmentId}");

        var mappedProductAttachment = this.mapper.Map<ProductAttachment>(dto);

        await this.repository.AddAsync(mappedProductAttachment,cancellationToken);
        await this.repository.SaveAsync(cancellationToken);

        mappedProductAttachment.Product = existProduct;
        mappedProductAttachment.Attachment = existAttachment;

        return this.mapper.Map<ProductAttachmentResultDto>(mappedProductAttachment);
    }

    public async Task<ProductAttachmentResultDto> UpdateAsync(ProductAttachmentUpdateDto dto,CancellationToken cancellationToken)
    {
        var existProductAttachment = await this.repository.GetAsync(p => p.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This productAttachment was not found with {dto.Id}");

        var existProduct = await this.productRepository.GetAsync(p => p.Id.Equals(dto.ProductId))
            ?? throw new NotFoundException($"This product was not found with {dto.ProductId}");

        var existAttachment = await this.attachmentRepository.GetAsync(p => p.Id.Equals(dto.AttachmentId))
            ?? throw new NotFoundException($"This attachment was not found with {dto.AttachmentId}");

        var mappedProductAttachment = this.mapper.Map(dto, existProductAttachment);

        this.repository.Update(mappedProductAttachment);
        await this.repository.SaveAsync(cancellationToken);

        return this.mapper.Map<ProductAttachmentResultDto>(mappedProductAttachment);
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var existProductAttachment = await this.repository.GetAsync(p => p.Id.Equals(id))
            ?? throw new NotFoundException($"This productAttachment was not found with {id}");

        this.repository.Delete(existProductAttachment);
        await this.repository.SaveAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(long productId, long attachmentId,CancellationToken cancellationToken)
    {
        var existProductAttachment = await this.repository.GetAsync(p => p.ProductId.Equals(productId)&&p.AttachmentId.Equals(attachmentId))
            ?? throw new NotFoundException($"This productAttachment was not found");

        this.repository.Delete(existProductAttachment);
        await this.repository.SaveAsync(cancellationToken);

        return true;
    }


    public async Task<ProductAttachmentResultDto> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var existProductAttachment = await this.repository.GetAsync(p => p.Id.Equals(id))
            ?? throw new NotFoundException($"This productAttachment was not found with {id}");

        return this.mapper.Map<ProductAttachmentResultDto>(existProductAttachment);
    }

    public async Task<IEnumerable<ProductAttachmentResultDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var productAttachments = await this.repository.GetAll().ToListAsync(cancellationToken:cancellationToken);

        return this.mapper.Map<IEnumerable<ProductAttachmentResultDto>>(productAttachments);
    }
}
