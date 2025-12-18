using SANProductService.Product.Domain.Entities;

namespace SANProductService.Product.Domain.Repositories.ProductRepository;

public interface IBrandRepository
{
    Task<Brand> CreateBrand(Brand request);
    Task<Brand> GetBrandById(Guid request);
    Task<Brand> UpdateBrand(Brand request);
    Task<Brand> DeleteBrand(Brand request);
}