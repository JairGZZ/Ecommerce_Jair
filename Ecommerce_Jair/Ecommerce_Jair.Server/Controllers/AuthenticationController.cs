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
            var result = await _authService.RegisterUserAsync(userDTO);

            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Created();
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync(LoginRequestDTO loginRequestDTO)
        {
            var result = await _authService.AuthenticateUserAsync(loginRequestDTO);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Data);

        }
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutUserAsync([FromBody] LogoutRequestDTO logoutRequestDTO)
        {
            var isSucces = await _authService.LogoutUserAsync(logoutRequestDTO.RefreshToken);
            if (!isSucces.Success)
            {
                return BadRequest(isSucces.Error);
            }
            return Ok("Cierre de sesion exitoso");
        }
      
        
        
        
    }

}
