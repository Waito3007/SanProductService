using SANProductService.Product.Application.Interfaces;
using SANProductService.Product.Application.Services;

namespace SANProductService.Product.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddScoped<IBrandService,BrandService>();
        
        return services;
    }
}