using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using notes.application.Interfaces;
using notes.application.Models.User;
using System.Threading.Tasks;

namespace notes.api.Controllers
{
    [ApiController]
    [Route("auth")]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpModel model)
        {
            var user = await _authenticationService.SignUpAsync(model);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInModel model)
        {
            var user = await _authenticationService.SignInAsync(model);
            return Ok(user);
        }
    }
}
