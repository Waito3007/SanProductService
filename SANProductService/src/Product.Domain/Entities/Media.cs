using SANProductService.Product.Domain.Enum;

namespace SANProductService.Product.Domain.Entities;

public class Media
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public string? AltText { get; set; }
    public MediaType Type { get; set; }
    
    public virtual ICollection<ProductMedia> ProductMedia { get; set; } = new HashSet<ProductMedia>();
    public virtual ICollection<VariantMedia> VariantMedia { get; set; } = new HashSet<VariantMedia>();
}

public class ProductMedia
{
    public Guid ProductId { get; set; }
    public Guid MediaId { get; set; }
    public int SortOrder { get; set; }
    
    public virtual Product Product { get; set; }
    public virtual Media Media { get; set; }
}

public class VariantMedia
{
    public Guid VariantId { get; set; }
    public Guid MediaId { get; set; }
    public int SortOrder { get; set; }
    
    public virtual Variant Variant { get; set; }
    public virtual Media Media { get; set; }
}