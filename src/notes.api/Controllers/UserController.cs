using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using notes.api.Extensions;
using notes.application.Interfaces;
using notes.application.Models.User;
using System.Threading.Tasks;

namespace notes.api.Controllers
{
    [ApiController]
    [Route("user")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var userId = HttpContext.GetSessionUserId();

            var user = await _userService.GetUserAsync(userId);

            return Ok(user);
        }


        [AllowAnonymous]
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var user = await _userService.ChangeUserPasswordAsync(model);

            return Ok(user);
        }
    }
}