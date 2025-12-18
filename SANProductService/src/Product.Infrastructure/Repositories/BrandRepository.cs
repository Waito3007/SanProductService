using Microsoft.EntityFrameworkCore;
using SANProductService.Product.Domain.Entities;
using SANProductService.Product.Domain.Repositories.ProductRepository;
using SANProductService.Product.Infrastructure.Data;

namespace SANProductService.Product.Infrastructure.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly ProductDbContext _context;

    public BrandRepository(ProductDbContext context)
    {
        _context = context;
    }
    
    public async Task<Brand> CreateBrand(Brand request)
    {
        _context.Brands.Add(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<Brand> GetBrandById(Guid request)
    {
        var result = await _context.Brands.FindAsync(request);
        return result;
    }

    public async Task<Brand> UpdateBrand(Brand request)
    {
        _context.Brands.Update(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<Brand> DeleteBrand(Brand request)
    {
        _context.Brands.Remove(request);
        await _context.SaveChangesAsync();
        return request;
    }
    
}