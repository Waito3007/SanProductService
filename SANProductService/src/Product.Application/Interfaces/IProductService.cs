using SANProductService.Product.Application.DTOs;
namespace SANProductService.Product.Application.Interfaces;

public interface IProductService
{
    Task<CreateResponse> CreateProductAsyns(CreateRequest request);
    // Task<UpdateProduct.UpdateResponse> UpdateProductAsync(UpdateProduct.UpdateRequest request);
}