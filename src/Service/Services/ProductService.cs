using AutoMapper;
using Data.IRepositories;
using Domain.Configuration;
using Domain.Entities.ProductFolder;
using Microsoft.EntityFrameworkCore;
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
    private readonly IMapper _mapper;
    private readonly IRepository<Product> _repository;
    private readonly IAttachmentService _attachmentService;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IProductAttachmentService _productAttachmentService;

    public ProductService(
        IMapper mapper,
        IRepository<Product> repository,
        IAttachmentService attachmentService,
        IRepository<Category> categoryRepository,
        IProductAttachmentService productAttachmentService)
    {
        _mapper = mapper;
        _repository = repository;
        _attachmentService = attachmentService;
        _categoryRepository = categoryRepository;
        _productAttachmentService = productAttachmentService;
    }

    public async Task<ProductResultDto> CreateAsync(ProductCreationDto dto)
    {
        var existCategory = await _categoryRepository.GetAsync(c => c.Id.Equals(dto.CategoryId))
            ?? throw new NotFoundException($"This category was not found with {dto.CategoryId}");

        if (await this.DoesProductExist(dto.Name))
            throw new AlreadyExistException($"Product with name '{dto.Name}' already exists.");

        var newProduct = _mapper.Map<Product>(dto);
        await _repository.AddAsync(newProduct);
        await _repository.SaveAsync();

        return _mapper.Map<ProductResultDto>(newProduct);
    }

    public async Task<IEnumerable<ProductResultDto>> RetrieveAllAsync(PaginationParams? paginationParams = null)
    {
        string[] inclusion = { "Category", "ProductAttachments.Attachment" };

        IQueryable<Product> query = _repository.GetAll(includes: inclusion);

        if (paginationParams is not null)
            query = query.ToPaginate(paginationParams);

        var products = await query.ToListAsync();

        return _mapper.Map<IEnumerable<ProductResultDto>>(products);
    }

    public async Task<ProductResultDto> RetrieveAsync(Expression<Func<Product, bool>> expression)
    {
        string[] inclusion = { "Category", "ProductAttachments.Attachment", "ProductItems" };
        
        var theProduct = await _repository.GetAsync(expression, inclusion)
            ?? throw new NotFoundException("Product with such properties is not found.");

        return _mapper.Map<ProductResultDto>(theProduct);
    }

    public async Task<ProductResultDto> RetrieveAsync(long id)
    {
        string[] inclusion = { "Category", "ProductAttachments.Attachment", "ProductItems" };

        var theProduct = await _repository.GetAsync(id, inclusion)
            ?? throw new NotFoundException("Product with such id is not found.");

        return _mapper.Map<ProductResultDto>(theProduct);
    }

    public async Task<IEnumerable<ProductResultDto>> RetrieveByCategoryIdAsync(long categoryId)
    {
        string[] inclusion = { "Category", "ProductAttachments.Attachment" };
        var products = await _repository.GetAll(p => p.CategoryId.Equals(categoryId), includes: inclusion).ToListAsync();

        return _mapper.Map<IEnumerable<ProductResultDto>>(products);
    }

    public async Task<ProductResultDto> ModifyAsync(ProductUpdateDto dto)
    {
        var theProduct = await _repository.GetAsync(dto.Id, new string[] { "Category", "ProductAttachments.Attachment"})
            ?? throw new NotFoundException("Product is not found.");

        var existCategory = await _categoryRepository.GetAsync(c => c.Id.Equals(dto.CategoryId))
            ?? throw new NotFoundException($"This category was not found with {dto.CategoryId}");

        if (!theProduct.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase))
            if(await this.DoesProductExist(dto.Name))
                throw new AlreadyExistException($"Product with name '{dto.Name}' already exists.");

        _mapper.Map(dto, theProduct);
        _repository.Update(theProduct);
        await _repository.SaveAsync();

        return _mapper.Map<ProductResultDto>(theProduct);
    }

    public async Task<bool> RemoveAsync(long id, bool destroy = false)
    {
        var theProduct = await _repository.GetAsync(id)
            ?? throw new NotFoundException("Product with such id is not found.");

        if (destroy)
            _repository.Destroy(theProduct);
        else
            _repository.Delete(theProduct);

        await _repository.SaveAsync();

        return true;
    }

    private async ValueTask<bool> DoesProductExist(string name)
    {
        var theProduct = await _repository.GetAsync(p => p.Name.ToLower() == name.ToLower());
        return theProduct != null;
    }

    public async Task<ProductResultDto> ImageUploadAsync(long productId, AttachmentCreationDto dto)
    {
        var existProduct = await _repository.GetAsync(p=> p.Id.Equals(productId), 
            new string[] { "Category", "ProductAttachments.Attachment"})
            ?? throw new NotFoundException($"This productId was not found with {productId}");
        
        var createdAttachment = await _attachmentService.UploadImageAsync(dto);

        if (existProduct.ProductAttachments.Any())
        {
            var productAttachment = existProduct.ProductAttachments.FirstOrDefault();
            long attachmentId = productAttachment.AttachmentId;

            await _attachmentService.DeleteImageAsync(attachmentId);
            await _productAttachmentService.DeleteAsync(productAttachment.Id);
        }

        var mappedProduct = _mapper.Map<ProductResultDto>(existProduct);

        var productAttachment2 = new ProductAttachmentCreationDto()
        {
            ProductId = productId,
            AttachmentId = createdAttachment.Id,
        };

        mappedProduct.ProductAttachments.Add(await _productAttachmentService.CreateAsync(productAttachment2));
        return mappedProduct;
    }
}
