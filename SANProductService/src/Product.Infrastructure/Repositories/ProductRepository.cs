using SANProductService.Product.Domain.Repositories.ProductRepository;
using SANProductService.Product.Infrastructure.Data;


namespace SANProductService.Product.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _context;
    
    public ProductRepository(ProductDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Product> AddAsync(Domain.Entities.Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }
    
}