using Domain.Entities.Addresses;
using Domain.Entities.AttachmentFolder;
using Domain.Entities.OrderFolder;
using Domain.Entities.Payment;
using Domain.Entities.ProductFolder;
using Domain.Entities.Shopping;
using Domain.Entities.UserFolder;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    //Addresses
    public DbSet<Region> Regions { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<District> Districts { get; set; }

    //Attachments
    public DbSet<Attachment> Attachments { get; set; }

    //Orders
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<OrderStatus> OrderStatuses { get; set; }
    public DbSet<ShippingMethod> ShippingMethods { get; set; }

    //Payments
    public DbSet<PaymentType> PaymentTypes { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }

    //Products
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Variation> Variations { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<ProductItem> ProductItems { get; set; }
    public DbSet<VariationOption> VarationOptions { get; set; }
    public DbSet<PromotionCategory> PromotionCategories { get; set; }
    public DbSet<ProductConfiguration> ProductConfigurations {get; set;}
    public DbSet<ProductAttachment> ProductAttachments {get; set; }
    public DbSet<ProductItemAttachment> ProductItemAttachments {get; set; }

    //Carts
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

    //Users
    public DbSet<User> Users { get; set; }
    public DbSet<UserReview> UserReviews { get; set; }
    public DbSet<UserAddress> UserAddresses { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Define
        var addresses = modelBuilder.Entity<Address>();
        var countries = modelBuilder.Entity<Country>();
        var districts = modelBuilder.Entity<District>();
        var regions = modelBuilder.Entity<Region>();
        var attachments = modelBuilder.Entity<Attachment>();
        var orders = modelBuilder.Entity<Order>();
        var orderLines = modelBuilder.Entity<OrderLine>();
        var orderStatuses = modelBuilder.Entity<OrderStatus>();
        var shippingMethods = modelBuilder.Entity<ShippingMethod>();
        var paymentMethods = modelBuilder.Entity<PaymentMethod>();
        var paymentTypes = modelBuilder.Entity<PaymentType>();
        var categories = modelBuilder.Entity<Category>();
        var products = modelBuilder.Entity<Product>();
        var productAttachments = modelBuilder.Entity<ProductAttachment>();
        var productConfigurations = modelBuilder.Entity<ProductConfiguration>();
        var productItems = modelBuilder.Entity<ProductItem>();
        var productItemAttachments = modelBuilder.Entity<ProductItemAttachment>();
        var promotions = modelBuilder.Entity<Promotion>();
        var promotionCategories = modelBuilder.Entity<PromotionCategory>();
        var variations = modelBuilder.Entity<Variation>();
        var variationOptions = modelBuilder.Entity<VariationOption>();
        var shoppingCarts = modelBuilder.Entity<ShoppingCart>();
        var shoppingCartItems = modelBuilder.Entity<ShoppingCartItem>();
        var users = modelBuilder.Entity<User>();
        var userAddresses = modelBuilder.Entity<UserAddress>();
        var userReviews = modelBuilder.Entity<UserReview>();
        #endregion

        #region Filter
        addresses.HasQueryFilter(e => !e.IsDeleted);
        countries.HasQueryFilter(e => !e.IsDeleted);
        districts.HasQueryFilter(e => !e.IsDeleted);
        regions.HasQueryFilter(e => !e.IsDeleted);
        attachments.HasQueryFilter(e => !e.IsDeleted);
        orders.HasQueryFilter(e => !e.IsDeleted);
        orderLines.HasQueryFilter(e => !e.IsDeleted);
        orderStatuses.HasQueryFilter(e => !e.IsDeleted);
        shippingMethods.HasQueryFilter(e => !e.IsDeleted);
        paymentMethods.HasQueryFilter(e => !e.IsDeleted);
        paymentTypes.HasQueryFilter(e => !e.IsDeleted);
        categories.HasQueryFilter(e => !e.IsDeleted);
        products.HasQueryFilter(e => !e.IsDeleted);
        productAttachments.HasQueryFilter(e => !e.IsDeleted);
        productConfigurations.HasQueryFilter(e => !e.IsDeleted);
        productItems.HasQueryFilter(e => !e.IsDeleted);
        productItemAttachments.HasQueryFilter(e => !e.IsDeleted);
        promotions.HasQueryFilter(e => !e.IsDeleted);
        promotionCategories.HasQueryFilter(e => !e.IsDeleted);
        variations.HasQueryFilter(e => !e.IsDeleted);
        variationOptions.HasQueryFilter(e => !e.IsDeleted);
        shoppingCarts.HasQueryFilter(e => !e.IsDeleted);
        shoppingCartItems.HasQueryFilter(e => !e.IsDeleted);
        users.HasQueryFilter(e => !e.IsDeleted);
        userAddresses.HasQueryFilter(e => !e.IsDeleted);
        userReviews.HasQueryFilter(e => !e.IsDeleted);
        #endregion

        #region Many to many relationship
        
        // Users <=> Addresses
        userAddresses.HasKey(ua => new { ua.UserId, ua.AddressId });
        userAddresses.HasOne(ua => ua.User).WithMany(ua => ua.UserAddresses).HasForeignKey(ua => ua.UserId);
        userAddresses.HasOne(ua => ua.Address).WithMany(ua => ua.UserAddresses).HasForeignKey(ua => ua.AddressId);

        // ProductItems <=> VariationOptions
        productConfigurations.HasKey(pc => new { pc.ProductItemId, pc.VariationOptionId });
        productConfigurations.HasOne(pc => pc.ProductItem).WithMany(pc => pc.ProductConfigurations).HasForeignKey(pc => pc.ProductItemId);
        productConfigurations.HasOne(pc => pc.VariationOption).WithMany(pc => pc.ProductConfigurations).HasForeignKey(pc => pc.VariationOptionId);

        // Promotions <=> ProductCategories
        promotionCategories.HasKey(pc => new { pc.PromotionId, pc.CategoryId });
        promotionCategories.HasOne(pc => pc.Promotion).WithMany(pc => pc.PromotionCategories).HasForeignKey(pc => pc.PromotionId);
        promotionCategories.HasOne(pc => pc.Category).WithMany(pc => pc.PromotionCategories).HasForeignKey(pc => pc.CategoryId);
        
        #endregion

        #region FluentApi
        #endregion

        #region SeedData
        #endregion
    }
}
