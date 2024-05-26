using AutoMapper;
using Data.IRepositories;
using Domain.Configuration;
using Domain.Entities.ProductFolder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.DTOs.Attachments;
using Service.DTOs.ProductAttachments;
using Service.DTOs.Products;
using Service.Exceptions;
using Service.Extensions;
using Service.Interfaces;
using System.Linq.Expressions;

namespace Service.Services;
public class ProductService : IProductService
{
    private readonly ILogger<ProductService> logger;
    private readonly IMapper mapper;
    private readonly IRepository<Product> productRepository;
    private readonly IAttachmentService attachmentService;
    private readonly IRepository<Category> categoryRepository;
    private readonly IProductAttachmentService productAttachmentService;

    public ProductService(
        ILogger<ProductService> logger,
        IMapper mapper,
        IRepository<Product> productRepository,
        IAttachmentService attachmentService,
        IRepository<Category> categoryRepository,
        IProductAttachmentService productAttachmentService)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.productRepository = productRepository;
        this.attachmentService = attachmentService;
        this.categoryRepository = categoryRepository;
        this.productAttachmentService = productAttachmentService;
    }

    public async Task<ProductResultDto> CreateAsync(ProductCreationDto dto, CancellationToken cancellationToken = default)
    {
        var existCategory = await this.categoryRepository.GetAsync(cr => cr.Id == dto.CategoryId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"This category was not found with {dto.CategoryId}");

        if (await this.doesProductExistAsync(dto.Name, cancellationToken))
            throw new AlreadyExistException($"Product with name '{dto.Name}' already exists.");

        var newProduct = this.mapper.Map<Product>(dto);
        await this.productRepository.AddAsync(newProduct, cancellationToken);
        await this.productRepository.SaveAsync(cancellationToken);

        return this.mapper.Map<ProductResultDto>(newProduct);
    }

    public async Task<IEnumerable<ProductResultDto>> RetrieveAllAsync(PaginationParams? paginationParams = null, CancellationToken cancellationToken = default)
    {
        string[] inclusion = { "Category", "ProductAttachments.Product" };

        IQueryable<Product> query = this.productRepository.GetAll(includes: inclusion);

        if (paginationParams is not null)
            query = query.ToPaginate(paginationParams);

        var products = await query.ToListAsync(cancellationToken: cancellationToken);

        return this.mapper.Map<IEnumerable<ProductResultDto>>(products);
    }

    public async Task<ProductResultDto> RetrieveAsync(Expression<Func<Product, bool>> expression, CancellationToken cancellationToken = default)
    {
        string[] inclusion = { "Category", "ProductAttachments.Attachment", "ProductItems" };
        
        var product = await this.productRepository.GetAsync(expression, inclusion, cancellationToken)
            ?? throw new NotFoundException("Product with such properties is not found.");

        return this.mapper.Map<ProductResultDto>(product);
    }

    public async Task<ProductResultDto> RetrieveAsync(long id, CancellationToken cancellationToken = default)
    {
        string[] inclusion = { "Category", "ProductAttachments.Attachment", "ProductItems" };

        var product = await this.productRepository.GetAsync(id, inclusion)
            ?? throw new NotFoundException("Product with such id is not found.");

        return this.mapper.Map<ProductResultDto>(product);
    }

    public async Task<IEnumerable<ProductResultDto>> RetrieveByCategoryIdAsync(long categoryId, CancellationToken cancellationToken = default)
    {
        string[] inclusion = { "Category", "ProductAttachments.Attachment" };
        var products = await this.productRepository
                .GetAll(pr => pr.CategoryId == categoryId, includes: inclusion)
                .ToListAsync(cancellationToken: cancellationToken);

        return this.mapper.Map<IEnumerable<ProductResultDto>>(products);
    }

    public async Task<ProductResultDto> ModifyAsync(ProductUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var product = await this.productRepository
                .GetAsync(dto.Id, new string[] { "Category", "ProductAttachments.Attachment"}, cancellationToken)
            ?? throw new NotFoundException("Product is not found.");

        var existCategory = await this.categoryRepository.GetAsync(cr => cr.Id == dto.CategoryId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"This category was not found with {dto.CategoryId}");

        if (!product.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase))
            if(await this.doesProductExistAsync(dto.Name, cancellationToken))
                throw new AlreadyExistException($"Product with name '{dto.Name}' already exists.");

        this.mapper.Map(dto, product);
        this.productRepository.Update(product);
        await this.productRepository.SaveAsync(cancellationToken);

        return this.mapper.Map<ProductResultDto>(product);
    }

    public async Task<bool> RemoveAsync(long id, bool destroy = false, CancellationToken cancellationToken = default)
    {
        var product = await this.productRepository.GetAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException("Product with such id is not found.");

        if (destroy)
            this.productRepository.Destroy(product);
        else
            this.productRepository.Delete(product);

        await this.productRepository.SaveAsync(cancellationToken);

        return true;
    }

    private async ValueTask<bool> doesProductExistAsync(string name, CancellationToken cancellationToken = default)
    {
        var product = await this.productRepository
            .GetAsync(p => p.Name.ToLower() == name.ToLower(), cancellationToken: cancellationToken);
        return product != null;
    }

    public async Task<ProductResultDto> UploadImageAsync(long productId, AttachmentCreationDto dto, CancellationToken cancellationToken = default)
    {
        var inclusion = new string[] { "Category", "ProductAttachments.Attachment" };

        var product = await this.productRepository
                .GetAsync(pr => pr.Id == productId, inclusion, cancellationToken)
            ?? throw new NotFoundException($"Product with id = {productId} is not found.");
        
        var newAttachment = await this.attachmentService.UploadImageAsync(dto);

        if (product.ProductAttachments.Any())
        {
            var productAttachment = product.ProductAttachments.First();
            var attachmentId = productAttachment.AttachmentId;

            await this.attachmentService.DeleteImageAsync(attachmentId, cancellationToken);
            await this.productAttachmentService.DeleteAsync(productAttachment.Id, cancellationToken);
        }

        var mappedProduct = this.mapper.Map<ProductResultDto>(product);

        var productAttachment2 = new ProductAttachmentCreationDto()
        {
            ProductId = productId,
            AttachmentId = newAttachment.Id,
        };

        mappedProduct.ProductAttachments.Add(await this.productAttachmentService.CreateAsync(productAttachment2, cancellationToken));
        return mappedProduct;
    }
}
