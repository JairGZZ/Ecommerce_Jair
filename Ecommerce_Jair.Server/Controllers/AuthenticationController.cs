using Ecommerce_Jair.Server.DTOs;
using Ecommerce_Jair.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Jair.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync(CreateUserDTO userDTO)
        {
            await _authService.RegisterUserAsync(userDTO);
            return Ok();
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync(LoginRequestDTO loginRequestDTO)
        {
            var response = await _authService.AuthenticateUserAsync(loginRequestDTO);
            return Ok(response);
        }
        
    }

}
