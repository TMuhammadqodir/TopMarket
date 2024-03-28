using Data.IRepositories;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service.Interfaces;
using Service.Mappers;
using Service.Services;
using System.Runtime.CompilerServices;
using System.Text;

namespace TopMarket.Extentions;

public static class ServicesCollection  
{
    public static void AddService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IPromotionService, PromotionService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IDistrictService, DistrictService>();
        services.AddScoped<IRegionService, RegionService>(); 
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<IVariationService, VariationService>();
        services.AddScoped<IVariationOptionService, VariationOptionService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductAttachmentService, ProductAttachmentService>();
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<IProductItemService, ProductItemService>();
        services.AddScoped<IProductItemAttachmentService, ProductItemAttachmentService>();
        services.AddScoped<IProductConfigurationService, ProductConfigurationService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IPromotionCategoryService, PromotionCategoryService>();
        services.AddScoped<IShippingMethodService, ShippingMethodService>();
        services.AddScoped<IOrderStatusService, OrderStatusService>();

    }
    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

    }
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(setup =>
        {
            // Include 'SecurityScheme' to use JWT Authentication
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
        });
    }
}
