using Microsoft.EntityFrameworkCore;
using SANProductService.Product.Domain.Entities;

namespace SANProductService.Product.Infrastructure.Data;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ProductTag> ProductTags { get; set; }
    public DbSet<Media> Media { get; set; }
    public DbSet<ProductMedia> ProductMedia { get; set; }
    public DbSet<Variant> Variants { get; set; }
    public DbSet<VariantMedia> VariantMedia { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<OptionValue> OptionValues { get; set; }
    public DbSet<VariantOptionValue> VariantOptionValues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Domain.Entities.Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Slug).IsRequired().HasMaxLength(250);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.IsPublished).HasDefaultValue(false);

            entity.HasOne(e => e.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(e => e.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.Slug).IsUnique();
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Slug).IsRequired().HasMaxLength(150);

            entity.HasOne(e => e.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(e => e.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.Slug).IsUnique();
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LogoUrl).HasMaxLength(500);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<ProductTag>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.TagId });

            entity.HasOne(e => e.Product)
                .WithMany(p => p.ProductTags)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Tag)
                .WithMany(t => t.ProductTags)
                .HasForeignKey(e => e.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Media>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Url).IsRequired().HasMaxLength(500);
            entity.Property(e => e.AltText).HasMaxLength(200);
            entity.Property(e => e.Type).IsRequired();
        });

        modelBuilder.Entity<ProductMedia>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.MediaId });

            entity.HasOne(e => e.Product)
                .WithMany(p => p.ProductMedia)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Media)
                .WithMany(m => m.ProductMedia)
                .HasForeignKey(e => e.MediaId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.SortOrder).HasDefaultValue(0);
        });

        modelBuilder.Entity<Variant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Sku).IsRequired().HasMaxLength(50);
            entity.Property(e => e.OriginalPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.SalePrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.StockQuantity).HasDefaultValue(0);

            entity.HasOne(e => e.Product)
                .WithMany(p => p.Variants)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.Sku).IsUnique();
        });

        modelBuilder.Entity<VariantMedia>(entity =>
        {
            entity.HasKey(e => new { e.VariantId, e.MediaId });

            entity.HasOne(e => e.Variant)
                .WithMany(v => v.VariantMedia)
                .HasForeignKey(e => e.VariantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Media)
                .WithMany(m => m.VariantMedia)
                .HasForeignKey(e => e.MediaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.SortOrder).HasDefaultValue(0);
        });

        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<OptionValue>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Value).IsRequired().HasMaxLength(100);

            entity.HasOne(e => e.Option)
                .WithMany(o => o.OptionValues)
                .HasForeignKey(e => e.OptionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<VariantOptionValue>(entity =>
        {
            entity.HasKey(e => new { e.VariantId, e.OptionValueId });

            entity.HasOne(e => e.Variant)
                .WithMany(v => v.VariantOptionValues)
                .HasForeignKey(e => e.VariantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.OptionValue)
                .WithMany(ov => ov.VariantOptionValues)
                .HasForeignKey(e => e.OptionValueId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}