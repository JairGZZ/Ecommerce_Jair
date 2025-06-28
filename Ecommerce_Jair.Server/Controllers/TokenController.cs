using Ecommerce_Jair.Server.Services.implementations;
using Ecommerce_Jair.Server.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ecommerce_Jair.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
     
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token,int userId)
        {
            var isValid =  await _tokenService.ValidateEmailConfirmationTokenAsync(userId, token);
            if (!isValid)
            {
                return BadRequest("Invalid or expired token.");
            }

            return Ok("Email confirmed.");

        }
       
   

        // POST api/<TokenController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TokenController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TokenController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
