using SANProductService.Product.Domain.Exceptions;
using SANProductService.Product.Domain.Extensions;

namespace SANProductService.Product.Application.DTOs;

public class ProductRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Slug { get; set; }
    
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrEmpty(Slug))
        {
            throw new ArgumentException(ResponseType.AttributeCannotBeEmpty.GetDescription());
        }
    }
}
public class ProductResponse
{
    
}