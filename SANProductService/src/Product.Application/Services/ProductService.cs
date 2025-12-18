using BackgroundLogService.Extensions;
using BackgroundLogService.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using SANProductService.Product.Application.DTOs;
using SANProductService.Product.Application.Interfaces;
using SANProductService.Product.Domain.Repositories.ProductRepository;

namespace SANProductService.Product.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IBackgroundLogService _backGroundlogService;
    private readonly ISessionLogService _sessonLogService;
    
    public ProductService(
        IProductRepository productRepository,
        IServiceProvider serviceProvider
        )
    {
        _productRepository = productRepository;
        _backGroundlogService = serviceProvider.GetLogService("SANProductService")!;
        _sessonLogService = serviceProvider.GetSessionLogService();
    }
    
    public async Task<CreateResponse> CreateProductAsyns(CreateRequest request)
    {
        try
        {
            var product = new Domain.Entities.Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Slug = request.Slug
            };
            
            var response = await _productRepository.AddAsync(product);
            
            await _backGroundlogService.WriteMessageAsync(_sessonLogService, $"Tạo sản phẩm thành công với ID: {product.Id}");
            
            return new CreateResponse();
        }
        catch (Exception ex)
        {
            await _backGroundlogService.WriteExceptionAsync(_sessonLogService, ex);
            await _backGroundlogService.WriteMessageAsync(_sessonLogService, ex.Message);
            throw;
        }
    }
}