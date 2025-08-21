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

        public TestController(ILogger<TestController> logger, ICategoryRepository categoryRepository, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _categoryService = categoryService;
        }
        // GET: TestController

        [HttpPost("Test")]
        public async Task<ActionResult<List<Category>>> Index(int id)
        {

          
            return Ok(await _categoryService.GetAllCategories());

        }

      
    }
}
