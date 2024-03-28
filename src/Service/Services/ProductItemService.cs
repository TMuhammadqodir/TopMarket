using AutoMapper;
using Data.IRepositories;
using Data.Repositories;
using Domain.Entities.AttachmentFolder;
using Domain.Entities.ProductFolder;
using Microsoft.EntityFrameworkCore;
using Service.DTOs.Attachments;
using Service.DTOs.ProductAttachments;
using Service.DTOs.ProductItemAttachments;
using Service.DTOs.ProductItems;
using Service.DTOs.Products;
using Service.Exceptions;
using Service.Helpers;
using Service.Interfaces;

namespace Service.Services;

public class ProductItemService : IProductItemService
{
    private readonly IMapper mapper;
    private readonly IRepository<ProductItem> repository;
    private readonly IVariationService variationService;
    private readonly IAttachmentService attachmentService;
    private readonly IRepository<Product> productRepository;
    private readonly IRepository<Category> categoryRepository;
    private readonly IProductItemAttachmentService productItemAttachmentService;
    public ProductItemService(
        IMapper mapper,
        IRepository<ProductItem> repository,
        IVariationService variationService,
        IAttachmentService attachmentService,
        IRepository<Product> productRepository,
        IRepository<Category> categoryRepository,
        IProductItemAttachmentService productItemAttachmentService)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.variationService = variationService;
        this.productRepository = productRepository;
        this.attachmentService = attachmentService;
        this.categoryRepository = categoryRepository;
        this.productItemAttachmentService = productItemAttachmentService;
    }

    public async Task<ProductItemResultDto> CreateAsync(ProductItemCreationDto dto)
    {
        var existProduct = await this.productRepository.GetAsync(c => c.Id.Equals(dto.ProductId))
            ?? throw new NotFoundException($"This product was not found with {dto.ProductId}");

        var mappedProductItem = this.mapper.Map<ProductItem>(dto);
        mappedProductItem.SKU = SKUHelper.GenerateSKU();
        mappedProductItem.QuantityInStock = 0;

        await this.repository.AddAsync(mappedProductItem);
        await this.repository.SaveAsync();

        mappedProductItem.Product = existProduct;

        return this.mapper.Map<ProductItemResultDto>(mappedProductItem);
    }

    public async Task<ProductItemResultDto> AddAsync(ProductItemIncomeDto dto)
    {
        var existProductItem = await this.repository.GetAsync(c => c.Id.Equals(dto.Id), new string[] { "Product" })
            ?? throw new NotFoundException($"This product was not found with {dto.Id}");

        existProductItem.QuantityInStock += dto.QuantityInStock;

        this.repository.Update(existProductItem);
        await this.repository.SaveAsync();

        return this.mapper.Map<ProductItemResultDto>(existProductItem);
    }

    public async Task<ProductItemResultDto> SubstractAsync(ProductItemIncomeDto dto)
    {
        var existProductItem = await this.repository.GetAsync(c => c.Id.Equals(dto.Id), new string[] { "Product" })
            ?? throw new NotFoundException($"This product was not found with {dto.Id}");

        if (existProductItem.QuantityInStock < dto.QuantityInStock)
            throw new CustomException(400,"ProductItem quantity is not enough");

        existProductItem.QuantityInStock -= dto.QuantityInStock;

        this.repository.Update(existProductItem);
        await this.repository.SaveAsync();

        return this.mapper.Map<ProductItemResultDto>(existProductItem);
    }


    public async Task<ProductItemResultDto> UpdateAsync(ProductItemUpdateDto dto)
    {
        var existProductItem = await this.repository.GetAsync(c => c.Id.Equals(dto.Id), includes: new[] { "Product", "ProductItemAttachments.Attachment" })
            ?? throw new NotFoundException($"This productItem was not found with {dto.Id}");

        var existProduct = await this.productRepository.GetAsync(c => c.Id.Equals(dto.ProductId))
            ?? throw new NotFoundException($"This product was not found with {dto.ProductId}");

        var mappedProductItem = this.mapper.Map(dto, existProductItem);

        this.repository.Update(mappedProductItem);
        await this.repository.SaveAsync();

        return this.mapper.Map<ProductItemResultDto>(mappedProductItem);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existProductItem = await this.repository.GetAsync(c => c.Id.Equals(id))
            ?? throw new NotFoundException($"This productItem was not found with {id}");

        this.repository.Delete(existProductItem);
        await this.repository.SaveAsync();

        return true;
    }

    public async Task<ProductItemResultDto> GetByIdAsync(long id)
    {
        var existProductItem = await this.repository.GetAsync(c => c.Id.Equals(id), 
            includes: new[] { "Product", "ProductItemAttachments.Attachment" })
            ?? throw new NotFoundException($"This productItem was not found with {id}");

        if (existProductItem.Product is null)
            throw new NotFoundException("This productItem's product not found");
        
        var categoryId = existProductItem.Product.CategoryId;

        var existCategory = await this.categoryRepository.GetAsync(c => c.Id.Equals(categoryId))
            ?? throw new NotFoundException("This productItem's category not found in productItemService");

        var result = this.mapper.Map<ProductItemResultDto>(existProductItem);
        result.Variations = (await variationService.GetFeaturesOfProduct(categoryId, id)).ToList();

        return result;
    }

    public async Task<IEnumerable<ProductItemResultDto>> GetAllAsync()
    {
        var productItems = await this.repository.GetAll(
            includes: new[] { "Product", "ProductItemAttachments.Attachment" }).ToListAsync();

        var resultProductItems = new List<ProductItemResultDto>();

        foreach (var productItem in productItems)
        {
            if (productItem.Product is null)
                continue;

            var categoryId = productItem.Product.CategoryId;

            var existCategory = await this.categoryRepository.GetAsync(c => c.Id.Equals(categoryId));

            if (existCategory is null)
                continue;

            var result = this.mapper.Map<ProductItemResultDto>(productItem);
            result.Variations = (await variationService.GetFeaturesOfProduct(categoryId, productItem.Id)).ToList();

            resultProductItems.Add(result);
        }

        return resultProductItems.AsEnumerable();
    }

    public async Task<ProductItemResultDto> AddImageAsync(long productItemId, AttachmentCreationDto dto)
    {
        var existProductItem = await this.repository.GetAsync(p => p.Id.Equals(productItemId), 
            new string[] {"Product", "ProductItemAttachments" })
            ?? throw new NotFoundException($"This productId was not found with {productItemId}");

        var createdAttachment = await this.attachmentService.UploadImageAsync(dto);

        var mappedProduct = this.mapper.Map<ProductItemResultDto>(existProductItem);

        var productItemAttachment = new ProductItemAttachmentCreationDto()
        {
            ProductItemId = productItemId,
            AttachmentId = createdAttachment.Id,
        };

        mappedProduct.ProductItemAttachments.Add(await this.productItemAttachmentService.CreateAsync(productItemAttachment));
        return mappedProduct;
    }

    public async Task<bool> DeleteImageAsync(long productItemId, long imageId)
    {
        var existProductItem = await this.repository.GetAsync(p => p.Id.Equals(productItemId),
            new string[] {"Product", "ProductItemAttachments.Attachment" })
            ?? throw new NotFoundException($"This productId was not found with {productItemId}");

        await attachmentService.DeleteImageAsync(imageId);
        await productItemAttachmentService.DeleteAsync(productItemId, imageId);

        var image = existProductItem.ProductItemAttachments.FirstOrDefault(p=>p.AttachmentId.Equals(imageId));

        return true;
    }

    public async Task<IEnumerable<ProductItemResultDto>> GetByProductIdAsync(long productId)
    {
        var existProductItems = await this.repository.GetAll(p => p.ProductId.Equals(productId),
            includes: new[] { "Product", "ProductItemAttachments.Attachment" }).ToListAsync();

        var resultProductItems = new List<ProductItemResultDto>();

        foreach (var productItem in existProductItems)
        {
            if (productItem.Product is null)
                throw new NotFoundException("This productItem's product not found");

            var categoryId = productItem.Product.CategoryId;

            var existCategory = await this.categoryRepository.GetAsync(c => c.Id.Equals(categoryId))
                ?? throw new NotFoundException("This productItem's category not found in productItemService");

            var result = this.mapper.Map<ProductItemResultDto>(productItem);
            result.Variations = (await variationService.GetFeaturesOfProduct(categoryId, productItem.Id)).ToList();

            resultProductItems.Add(result);
        }

        return resultProductItems.AsEnumerable();
    }
}
