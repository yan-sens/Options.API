using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Options.DbContext.Models;
using Options.Domain.Models;
using Options.Repositories.Contracts;

namespace Options.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserProfileRepository _userProfileRepository;

        public AccountController(ILogger<AccountController> logger, IMapper mapper, UserManager<IdentityUser> userManager, IUserProfileRepository userProfileRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _userProfileRepository = userProfileRepository;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> Get()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return BadRequest();

            UserProfile userProfile;
            var response = await _userProfileRepository.GetUserProfileByIdAsync(Guid.Parse(currentUser.Id));
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                userProfile = new UserProfile
                {
                    Id = Guid.Parse(currentUser.Id),
                    Name = currentUser.UserName ?? string.Empty,
                    Email = currentUser.Email ?? string.Empty,
                    PhoneNumber = currentUser.PhoneNumber ?? string.Empty
                };

                await _userProfileRepository.CreateUserProfileAsync(userProfile);
            }
            else
            {
                userProfile = response.Data!;
            }

            return Ok(userProfile);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserProfileAsync(UserProfileRequestModel model)
        {
            var mappedProfile = _mapper.Map<UserProfile>(model);
            var response = await _userProfileRepository.CreateUserProfileAsync(mappedProfile);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserProfileAsync(UserProfileRequestModel model)
        {
            var mappedProfile = _mapper.Map<UserProfile>(model);
            var response = await _userProfileRepository.UpdateUserProfileAsync(mappedProfile);
            return Ok(response);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteOptionAsync(Guid userId)
        {
            var response = await _userProfileRepository.DeleteUserProfileAsync(userId);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode);
            }

            return Ok(response);
        }
    }
}
