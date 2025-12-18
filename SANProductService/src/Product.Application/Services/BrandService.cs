using BackgroundLogService.Extensions;
using BackgroundLogService.Services.Interfaces;
using SANProductService.Product.Application.DTOs.Branch;
using SANProductService.Product.Application.Interfaces;
using SANProductService.Product.Domain.Entities;
using SANProductService.Product.Domain.Exceptions;
using SANProductService.Product.Domain.Repositories.ProductRepository;

namespace SANProductService.Product.Application.Services;

public class BrandService : IBrandService
{
    private readonly IBrandRepository _brandRepository;
    private readonly IBackgroundLogService _backGroundlogService;
    private readonly ISessionLogService _sessonLogService;
    
    public BrandService(IBrandRepository brandRepository, IServiceProvider serviceProvider)
    {
        _brandRepository = brandRepository;
        _backGroundlogService = serviceProvider.GetLogService("SANProductService")!;
        _sessonLogService = serviceProvider.GetSessionLogService();
    }

    public async Task<CreateBranchResponse> CreateBrand(CreateBranchRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ProjectException(ResponseType.NameCannotBeEmpty);
            }

            if (string.IsNullOrWhiteSpace(request.LogoUrl))
            {
                throw new ProjectException(ResponseType.ImageCannotBeEmpty);
            }
        
            var brand = new Brand
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                LogoUrl = request.LogoUrl
            };
        
            await _brandRepository.CreateBrand(brand);
        
            return new CreateBranchResponse {};
        }
        catch (Exception ex)
        {
            await _backGroundlogService.WriteExceptionAsync(_sessonLogService, ex);
            await _backGroundlogService.WriteMessageAsync(_sessonLogService, ex.Message);
            throw;
        }
        
    }
}