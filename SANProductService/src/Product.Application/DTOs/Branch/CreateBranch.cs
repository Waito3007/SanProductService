using SANProductService.Product.Domain.Exceptions;

namespace SANProductService.Product.Application.DTOs.Branch;

public class CreateBranchRequest
{
    public string Name { get; set; }
    public string? LogoUrl { get; set; }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ProjectException(ResponseType.NameCannotBeEmpty);
        }

        if (string.IsNullOrWhiteSpace(LogoUrl))
        {
            throw new ProjectException(ResponseType.ImageCannotBeEmpty);
        }
    }
}

public class CreateBranchResponse
{

}