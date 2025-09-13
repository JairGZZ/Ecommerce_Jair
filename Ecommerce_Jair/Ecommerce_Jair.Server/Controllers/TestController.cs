using Ecommerce_Jair.Server.DTOs.Product;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Repositories.Interfaces;
using Ecommerce_Jair.Server.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Engines;

namespace Ecommerce_Jair.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    //este controlador solo lo usare para debug
    public class TestController : ControllerBase
    {
        private ILogger<TestController> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public TestController(ILogger<TestController> logger, ICategoryRepository categoryRepository, ICategoryService categoryService, IProductService productService)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _categoryService = categoryService;
            _productService = productService;
            
        }
        // GET: TestController

        [HttpPost("Test")]
        public async Task<IActionResult> Index([FromQuery] ProductListCriteria criteria,CancellationToken ct)
        {
            var result = await _productService.GetAllProducts(criteria, ct);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Data);

        }

      
    }
}
