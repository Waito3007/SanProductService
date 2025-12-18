namespace SANProductService.Product.Domain.Entities;

public class Brand
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? LogoUrl { get; set; }
    
    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
}

