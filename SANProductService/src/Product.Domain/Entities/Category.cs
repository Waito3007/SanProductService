namespace SANProductService.Product.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    
    public Guid? ParentCategoryId { get; set; }
    
    public virtual Category? ParentCategory { get; set; }
    public virtual ICollection<Category> SubCategories { get; set; } = new HashSet<Category>();
    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
}