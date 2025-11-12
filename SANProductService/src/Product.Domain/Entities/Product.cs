namespace SANProductService.Product.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Slug { get; set; }
    public bool IsPublished { get; set; }
    
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }
    
    public virtual Category Category { get; set; }
    public virtual Brand Brand { get; set; }
    public virtual ICollection<Variant> Variants { get; set; } = new HashSet<Variant>();
    public virtual ICollection<ProductTag> ProductTags { get; set; } = new HashSet<ProductTag>();
    public virtual ICollection<ProductMedia> ProductMedia { get; set; } = new HashSet<ProductMedia>();
}

