using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Options.DbContext.Models;
using Options.Domain.Models;
using Options.Repositories.Contracts;
using Options.Repositories.Models;

namespace Options.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/options")]
    public class OptionsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IOptionsRepository _optionsRepository;

        public OptionsController(IMapper mapper, IOptionsRepository optionsRepository)
        {
            _mapper = mapper;
            _optionsRepository = optionsRepository;
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetOptionsAsync(OptionsFilter filter)
        {
            var response = await _optionsRepository.GetOptionsAsync(filter);

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOptionAsync(OptionRequestModel model)
        {
            var mappedOption = _mapper.Map<Option>(model);
            var response = await _optionsRepository.CreateOptionAsync(mappedOption);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOptionAsync(OptionRequestModel model)
        {
            model.ReturnAmount = model.Completed ? model.ReturnAmount : model.Worth;
            var mappedOption = _mapper.Map<Option>(model);
            var response = await _optionsRepository.UpdateOptionAsync(mappedOption);
            return Ok(response);
        }
    }
}
