using SANProductService.Product.Application.DTOs.Branch;

namespace SANProductService.Product.Application.Interfaces;

public interface IBrandService
{
    Task<CreateBranchResponse> CreateBrand(CreateBranchRequest request);
}