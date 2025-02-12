using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Options.DbContext.Models;

namespace Options.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(ILogger<AccountController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> Get()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return BadRequest();

            var response = new UserProfile
            {
                Id = Guid.Parse(currentUser.Id),
                Name = currentUser.UserName ?? string.Empty,
                Email = currentUser.Email ?? string.Empty,
                PhoneNumber = currentUser.PhoneNumber ?? string.Empty
            };

            return Ok(response);
        }
    }
}
