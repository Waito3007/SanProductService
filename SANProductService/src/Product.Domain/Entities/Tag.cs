namespace SANProductService.Product.Domain.Entities;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; } // Ví dụ: "new-arrival", "sale-off"
    
    public virtual ICollection<ProductTag> ProductTags { get; set; } = new HashSet<ProductTag>();
}

public class ProductTag
{
    // Composite Primary Key
    public Guid ProductId { get; set; }
    public Guid TagId { get; set; }

    // Navigation Properties
    public virtual Product Product { get; set; }
    public virtual Tag Tag { get; set; }
}