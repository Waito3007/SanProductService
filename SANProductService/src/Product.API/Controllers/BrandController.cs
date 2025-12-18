using Microsoft.AspNetCore.Mvc;
using SANProductService.Product.Application.DTOs.Branch;
using SANProductService.Product.Application.Interfaces;
using SANProductService.Product.Domain.Exceptions;

namespace SANProductService.src.Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> CreateBrand([FromBody] CreateBranchRequest request)
        {
            var data = await _brandService.CreateBrand(request);
            
            // Trả về success response
            var response = new CreateBranchResponse();
            
            return Ok(response);
        }
    }
}