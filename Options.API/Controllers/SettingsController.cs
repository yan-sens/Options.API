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
    [Route("api/settings")]
    public class SettingsController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ISettingsRepository _settingsRepository;

        public SettingsController(ILogger<AccountController> logger, IMapper mapper, UserManager<IdentityUser> userManager, IUserProfileRepository userProfileRepository, ISettingsRepository settingsRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _userProfileRepository = userProfileRepository;
            _settingsRepository = settingsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return BadRequest();

            Settings settings;
            var response = await _settingsRepository.GetSettingByUserIdAsync(Guid.Parse(currentUser.Id));
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                settings = new Settings
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.Parse(currentUser.Id),
                    RegulatoryFee = 0,
                    Tax = 0
                };

                await _settingsRepository.CreateSettingAsync(settings);
            }
            else
            {
                settings = response.Data!;
            }

            return Ok(settings);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserProfileAsync(UpdateSettingsRequestModel model)
        {
            var mappedSettings = _mapper.Map<Settings>(model);
            var response = await _settingsRepository.UpdateSettingAsync(mappedSettings);
            return Ok(response);
        }
    }
}
