namespace SANProductService.Product.Domain.Entities;

public class Variant
{
    public Guid Id { get; set; }
    public string Sku { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal? SalePrice { get; set; }
    public int StockQuantity { get; set; }
    
    public Guid ProductId { get; set; }
    
    public virtual Product Product { get; set; }
    public virtual ICollection<VariantOptionValue> VariantOptionValues { get; set; } = new HashSet<VariantOptionValue>();
    public virtual ICollection<VariantMedia> VariantMedia { get; set; } = new HashSet<VariantMedia>();
}

public class Option
{
    public Guid Id { get; set; }
    public string Name { get; set; } // Ví dụ: "Màu sắc", "Kích thước", "CPU"

    // Navigation Property
    public virtual ICollection<OptionValue> OptionValues { get; set; } = new HashSet<OptionValue>();
}

public class OptionValue
{
    public Guid Id { get; set; }
    public string Value { get; set; } // Ví dụ: "Xanh", "L", "Core i7"

    // Foreign Key
    public Guid OptionId { get; set; }

    // Navigation Properties
    public virtual Option Option { get; set; }
    public virtual ICollection<VariantOptionValue> VariantOptionValues { get; set; } = new HashSet<VariantOptionValue>();
}

public class VariantOptionValue
{
    // Composite Primary Key (sẽ được cấu hình trong DbContext)
    public Guid VariantId { get; set; }
    public Guid OptionValueId { get; set; }

    // Navigation Properties
    public virtual Variant Variant { get; set; }
    public virtual OptionValue OptionValue { get; set; }
}