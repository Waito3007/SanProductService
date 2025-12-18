using SANProductService.Product.Domain.Exceptions;
using SANProductService.Product.Domain.Extensions;

namespace SANProductService.Product.Application.DTOs;

public class CreateRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Slug { get; set; }
    
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ProjectException(ResponseType.NameCannotBeEmpty);
        }
    }
}

public class CreateResponse
{
    
}