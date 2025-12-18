using SANProductService.Product.Domain.Entities;
namespace SANProductService.Product.Domain.Repositories.ProductRepository;

public interface IProductRepository
{
    Task<Entities.Product> AddAsync(Entities.Product product);
}