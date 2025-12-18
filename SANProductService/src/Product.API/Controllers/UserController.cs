using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SANProductService.Product.Infrastructure.Repositories;

namespace SANProductService.src.Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository _productRepository;
        public ProductController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost("add")]
        public IActionResult AddProduct(SANProductService.Product.Domain.Entities.Product product)
        {
            //
            return Ok();

        }
    }
}
