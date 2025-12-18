using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SANProductService.Product.Domain.Repositories.ProductRepository;
using SANProductService.Product.Infrastructure.Data;
using SANProductService.Product.Infrastructure.Repositories;

namespace SANProductService.Product.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection service, 
        IConfiguration configuration)
    {
        //Sql Service 
        service.AddDbContext<ProductDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        //DI for Repository
        service.AddScoped<IBrandRepository, BrandRepository>();
        service.AddScoped<IProductRepository, ProductRepository>();
        return service;
    }
}