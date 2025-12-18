using SANProductService.Product.Domain.Exceptions;

namespace SANProductService.Product.Application.DTOs;

public class UpdateProduct
{
    public class UpdateRequest
    {
        private string Id { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                throw new ProjectException(ResponseType.IDCannotBeEmpty);
            }
        }
    }

    public class UpdateResponse
    {
        
    }
}