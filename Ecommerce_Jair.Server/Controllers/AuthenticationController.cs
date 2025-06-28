using Ecommerce_Jair.Server.DTOs;
using Ecommerce_Jair.Server.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
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
            return Ok(await _authService.AuthenticateUserAsync(loginRequestDTO));

        }
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutUserAsync([FromBody] LogoutRequestDTO logoutRequestDTO)
        {
            await _authService.LogoutUserAsync(logoutRequestDTO.RefreshToken);
            return Ok();
        }
      
        
        
        
    }

}
