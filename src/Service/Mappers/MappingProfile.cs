using AutoMapper;
using Domain.Entities.Addresses;
using Domain.Entities.AttachmentFolder;
using Domain.Entities.OrderFolder;
using Domain.Entities.ProductFolder;
using Domain.Entities.Shopping;
using Domain.Entities.UserFolder;
using Service.DTOs.Addresses;
using Service.DTOs.Attachments;
using Service.DTOs.Carts;
using Service.DTOs.Categories;
using Service.DTOs.Countries;
using Service.DTOs.Districts;
using Service.DTOs.OrderStates;
using Service.DTOs.ProductAttachments;
using Service.DTOs.ProductConfigurations;
using Service.DTOs.ProductItemAttachments;
using Service.DTOs.ProductItems;
using Service.DTOs.Products;
using Service.DTOs.PromotionCategories;
using Service.DTOs.Promotions;
using Service.DTOs.Regions;
using Service.DTOs.ShippingMethods;
using Service.DTOs.Users;
using Service.DTOs.VariationOptions;
using Service.DTOs.Variations;

namespace Service.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Attachment
        CreateMap<Attachment, AttachmentCreationDto>().ReverseMap();
        CreateMap<Attachment, AttachmentResultDto>().ReverseMap();

        //Category
        CreateMap<Category, CategoryCreationDto>().ReverseMap();
        CreateMap<Category, CategoryResultDto>().ReverseMap();
        CreateMap<Category, CategoryUpdateDto>().ReverseMap();

        //ProductAttachment
        CreateMap<ProductAttachment, ProductAttachmentCreationDto>().ReverseMap();
        CreateMap<ProductAttachment, ProductAttachmentResultDto>().ReverseMap();

        //ProductConfiguration
        CreateMap<ProductConfiguration, ProductConfigurationCreationDto>().ReverseMap();
        CreateMap<ProductConfiguration, ProductConfigurationResultDto>().ReverseMap();
        CreateMap<ProductConfiguration, ProductConfigurationUpdateDto>().ReverseMap();

        //ProductItemAttachment
        CreateMap<ProductItemAttachment, ProductItemAttachmentCreationDto>().ReverseMap();
        CreateMap<ProductItemAttachment, ProductItemAttachmentResultDto>().ReverseMap();

        //ProductItem
        CreateMap<ProductItem, ProductItemCreationDto>().ReverseMap();
        CreateMap<ProductItem, ProductItemResultDto>().ReverseMap();
        CreateMap<ProductItem, ProductItemUpdateDto>().ReverseMap();
        CreateMap<ProductItem, ProductItemIncomeDto>().ReverseMap();

        //Product
        CreateMap<Product, ProductCreationDto>().ReverseMap();
        CreateMap<Product, ProductResultDto>().ReverseMap();
        CreateMap<Product, ProductUpdateDto>().ReverseMap();

        //PromotionCategory
        CreateMap<PromotionCategory, PromotionCategoryCreationDto>().ReverseMap();
        CreateMap<PromotionCategory, PromotionCategoryResultDto>().ReverseMap();
        CreateMap<PromotionCategory, PromotionCategoryUpdateDto>().ReverseMap();

        //Promotion
        CreateMap<Promotion, PromotionCreationDto>().ReverseMap();
        CreateMap<Promotion, PromotionResultDto>().ReverseMap();
        CreateMap<Promotion, PromotionUpdateDto>().ReverseMap();

        //VariationOption
        CreateMap<VariationOption, VariationOptionCreationDto>().ReverseMap();
        CreateMap<VariationOption, VariationOptionResultDto>().ReverseMap();
        CreateMap<VariationOption, VariationOptionUpdateDto>().ReverseMap();
        CreateMap<VariationOption, VariationOptionFeatureResult>().ReverseMap();

        //Variation
        CreateMap<Variation, VariationCreationDto>().ReverseMap();
        CreateMap<Variation, VariationResultDto>().ReverseMap();
        CreateMap<Variation, VariationUpdateDto>().ReverseMap();
        CreateMap<Variation, VariationFeatureResultDto>().ReverseMap();

        //User
        CreateMap<User, UserCreationDto>().ReverseMap();
        CreateMap<User,UserUpdateDto>().ReverseMap();
        CreateMap<User,UserResultDto>().ReverseMap();
       
        // Country
        CreateMap<Country, CountryCreationDto>().ReverseMap();
        CreateMap<CountryResultDto, Country>().ReverseMap();

        // Region
        CreateMap<Region, RegionCreationDto>().ReverseMap();
        CreateMap<RegionResultDto, Region>().ReverseMap();

        // District
        CreateMap<District, DistrictCreationDto>().ReverseMap();
        CreateMap<DistrictResultDto, District>().ReverseMap();
        
        //Address
        CreateMap<Address, AddressCreationDto>().ReverseMap();
        CreateMap<Address, AddressResultDto>().ReverseMap();
        CreateMap<Address, AddressUpdateDto>().ReverseMap();

        //OrderStatus
        CreateMap<OrderStatus, OrderStatusCreationDto>().ReverseMap();
        CreateMap<OrderStatus, OrderStatusResultDto>().ReverseMap();
        CreateMap<OrderStatus, OrderStatusUpdateDto>().ReverseMap();

        //ShippingMethod
        CreateMap<ShippingMethod, ShippingMethodCreationDto>().ReverseMap();
        CreateMap<ShippingMethod, ShippingMethodResultDto>().ReverseMap();
        CreateMap<ShippingMethod, ShippingMethodUpdateDto>().ReverseMap();

        //Shopping cart
        CreateMap<ShoppingCart,CartResultDto>().ReverseMap();
    }
}
