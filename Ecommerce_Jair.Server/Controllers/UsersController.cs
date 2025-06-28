
using Microsoft.AspNetCore.Mvc;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Services.Interfaces;
using Ecommerce_Jair.Server.DTOs;

namespace Ecommerce_Jair.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public UsersController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        // GET: api/Users
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<ShowUserDTO>>> GetAllUsers()

        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("GetUserById/{id}")]
        public async Task<ActionResult<ShowUserDTO>> GetUser(int id)
        {

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        // POST: api/Users
        [HttpPost("CreateUser")]
        public async Task<ActionResult<User>> CreateUser(User user)
        {

            await _userService.CreateUserAsync(user);
            return Ok("SE REGISTRO CORRECTAMENTE EL USUARIO"); 
        }

        // PUT: api/Users/5
        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, user);
            if (!updatedUser) return NotFound();
            return Ok("SE ACTUALIZO CORRECTAMENTE EL USUARIO");
        }

        // DELETE: api/Users/5
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            await _userService.DeleteUserAsync(id);
            return Ok("SE ELIMINO CORRECTAMENTE EL USUARIO");
        }
    }

}
