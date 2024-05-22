using AutoMapper;
using Data.IRepositories;
using Domain.Entities.ProductFolder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.DTOs.Attachments;
using Service.DTOs.ProductItemAttachments;
using Service.DTOs.ProductItems;
using Service.Exceptions;
using Service.Helpers;
using Service.Interfaces;

namespace Service.Services;

public class ProductItemService : IProductItemService
{
    private readonly ILogger<ProductItemService> logger;
    private readonly IMapper mapper;
    private readonly IRepository<ProductItem> repository;
    private readonly IVariationService variationService;
    private readonly IAttachmentService attachmentService;
    private readonly IRepository<Product> productRepository;
    private readonly IRepository<Category> categoryRepository;
    private readonly IProductItemAttachmentService productItemAttachmentService;

    public ProductItemService(
        ILogger<ProductItemService> logger,
        IMapper mapper,
        IRepository<ProductItem> repository,
        IVariationService variationService,
        IAttachmentService attachmentService,
        IRepository<Product> productRepository,
        IRepository<Category> categoryRepository,
        IProductItemAttachmentService productItemAttachmentService)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.repository = repository;
        this.variationService = variationService;
        this.productRepository = productRepository;
        this.attachmentService = attachmentService;
        this.categoryRepository = categoryRepository;
        this.productItemAttachmentService = productItemAttachmentService;
    }

    public async Task<ProductItemResultDto> CreateAsync(ProductItemCreationDto dto, CancellationToken cancellationToken = default)
    {
        var existProduct = await this.productRepository.GetAsync(c => c.Id.Equals(dto.ProductId), cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"This product was not found with {dto.ProductId}");

        var mappedProductItem = this.mapper.Map<ProductItem>(dto);
        mappedProductItem.SKU = SKUHelper.GenerateSKU();
        mappedProductItem.QuantityInStock = 0;

        await this.repository.AddAsync(mappedProductItem, cancellationToken);
        await this.repository.SaveAsync(cancellationToken);

        mappedProductItem.Product = existProduct;

        return this.mapper.Map<ProductItemResultDto>(mappedProductItem);
    }

    public async Task<ProductItemResultDto> AddAsync(ProductItemIncomeDto dto, CancellationToken cancellationToken = default)
    {
        var inclusion = new string[] { "Product" };

        var productItem = await this.repository.GetAsync(dto.Id, inclusion, cancellationToken)
            ?? throw new NotFoundException($"Product item with id = {dto.Id} is not found.");

        productItem.QuantityInStock += dto.QuantityInStock;

        this.repository.Update(productItem);
        await this.repository.SaveAsync(cancellationToken);

        return this.mapper.Map<ProductItemResultDto>(productItem);
    }

    public async Task<ProductItemResultDto> SubstractAsync(ProductItemIncomeDto dto, CancellationToken cancellationToken = default)
    {
        var existProductItem = await this.repository.GetAsync(dto.Id, new string[] { "Product" }, cancellationToken)
            ?? throw new NotFoundException($"This product was not found with {dto.Id}");

        if (existProductItem.QuantityInStock < dto.QuantityInStock)
            throw new CustomException(400, "ProductItem quantity is not enough");

        existProductItem.QuantityInStock -= dto.QuantityInStock;

        this.repository.Update(existProductItem);
        await this.repository.SaveAsync(cancellationToken);

        return this.mapper.Map<ProductItemResultDto>(existProductItem);
    }

    public async Task<ProductItemResultDto> ModifyAsync(ProductItemUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var inclusion = new string[] { "Product" };

        var productItem = await this.repository.GetAsync(dto.Id, inclusion, cancellationToken)
            ?? throw new NotFoundException($"Product item with id = {dto.Id} is not found.");

        var mappedProductItem = this.mapper.Map(dto, productItem);

        this.repository.Update(mappedProductItem);
        await this.repository.SaveAsync(cancellationToken);

        return this.mapper.Map<ProductItemResultDto>(mappedProductItem);
    }

    public async Task<bool> RemoveAsync(long id, bool destroy = false, CancellationToken cancellationToken = default)
    {
        var productItem = await this.repository.GetAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Product item with id = {id} is not found.");

        try
        {
            if (destroy)
                this.repository.Destroy(productItem);
            else
                this.repository.Delete(productItem);
            
            await this.repository.SaveAsync(cancellationToken);
            this.logger.LogInformation("Product item has been successfully {action}.", destroy ? "destroyed" : "deleted");

            return true;
        }
        catch (Exception ex)
        {
            this.logger.LogError("Product item has NOT been deleted. See details: {@ex}", ex);
            return false;
        }
    }

    public async Task<ProductItemResultDto> RetrieveByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var inclusion = new string[] { "Product", "ProductItemAttachments.Attachment" };
        
        var productItem = await this.repository.GetAsync(id, includes: inclusion, cancellationToken)
            ?? throw new NotFoundException($"Product item with id = {id} is not found.");

        var result = this.mapper.Map<ProductItemResultDto>(productItem);
        result.Variations = (await this.variationService.GetFeaturesOfProduct(productItem.Product.CategoryId, productItem.Id)).ToList();

        return result;
    }

    public async Task<IEnumerable<ProductItemResultDto>> RetrieveAllAsync(CancellationToken cancellationToken = default)
    {
        var inclusion = new string[] { "Product", "ProductItemAttachments.Attachment" };
        
        var productItems = await this.repository
                .GetAll(includes: inclusion)
                .ToListAsync(cancellationToken: cancellationToken);

        var resultProductItems = new List<ProductItemResultDto>();

        foreach (var productItem in productItems)
        {
            //TODO Muhammadqodir: Soddalashtirish va optimizatsiya qilish kerak
            if (productItem.Product is null)
                continue;
            
            var category = await this.categoryRepository
                    .GetAsync(productItem.Product.CategoryId, cancellationToken: cancellationToken);

            if (category is null)
                continue;

            var result = this.mapper.Map<ProductItemResultDto>(productItem);
            result.Variations = (await variationService.GetFeaturesOfProduct(productItem.Product.CategoryId, productItem.Id)).ToList();

            resultProductItems.Add(result);
        }

        return resultProductItems.AsEnumerable();
    }

    public async Task<ProductItemResultDto> UploadImageAsync(long productItemId, AttachmentCreationDto dto, CancellationToken cancellationToken = default)
    {
        var inclusion = new string[] { "Product", "ProductItemAttachments" };
        
        var productItem = await this.repository.GetAsync(productItemId, inclusion, cancellationToken)
            ?? throw new NotFoundException($"Product item with id = {productItemId} is not found.");

        var newAttachment = await this.attachmentService.UploadImageAsync(dto);

        var mappedProductItem = this.mapper.Map<ProductItemResultDto>(productItem);

        var productItemAttachment = new ProductItemAttachmentCreationDto()
        {
            ProductItemId = productItemId,
            AttachmentId = newAttachment.Id,
        };

        mappedProductItem.ProductItemAttachments.Add(await this.productItemAttachmentService.CreateAsync(productItemAttachment));
        return mappedProductItem;
    }

    public async Task<bool> RemoveImageAsync(long productItemId, long imageId, CancellationToken cancellationToken = default)
    {
        try
        {
            await attachmentService.DeleteImageAsync(imageId);
            await productItemAttachmentService.DeleteAsync(productItemId, imageId);

            this.logger.LogInformation("Image has been successfully deleted.");

            return true;
        }
        catch (Exception ex)
        {
            this.logger.LogError("Image has NOT been removed. See details: {@exception}", ex);
            return false;
        }
    }

    public async Task<IEnumerable<ProductItemResultDto>> RetrieveByProductIdAsync(long productId, CancellationToken cancellationToken = default)
    {
        var inclusion = new string[] { "Product", "ProductItemAttachments" };

        var productItems = await this.repository
                .GetAll(pi => pi.ProductId == productId, includes: inclusion)
                .ToListAsync(cancellationToken);

        var resultProductItems = new List<ProductItemResultDto>();

        foreach (var productItem in productItems)
        {
            //TODO Muhammadqodir: Buniyam soddalashtirish va optimizatsiya qilish kerak

            if (productItem.Product is null) continue;

            var categoryId = productItem.Product.CategoryId;

            var category = await this.categoryRepository.GetAsync(categoryId, cancellationToken: cancellationToken);
            if (category is null) continue;

            var result = this.mapper.Map<ProductItemResultDto>(productItem);
            result.Variations = (await variationService.GetFeaturesOfProduct(categoryId, productItem.Id)).ToList();

            resultProductItems.Add(result);
        }

        return resultProductItems.AsEnumerable();
    }
}
