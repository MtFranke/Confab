using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Confab.Modules.Conferences.Core.DTO;
using Confab.Modules.Conferences.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Confab.Modules.Conferences.Api.Controllers
{
    [Authorize(Policy =  Policy)]
    internal class ConferencesController : BaseController
    {
        private const string Policy = "conferences";
        private readonly IConferenceService _conferenceService;

        public ConferencesController(IConferenceService conferenceService)
        {
            _conferenceService = conferenceService;
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<ConferenceDetailsDto>> Get(Guid id) 
            => OkOrNotFound(await _conferenceService.GetAsync(id));
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IReadOnlyList<HostDetailsDto>>> BrowseAsync() 
            => Ok(await  _conferenceService.BrowseAsync());

        [HttpPost]
        public async Task<ActionResult> AddAsync(ConferenceDetailsDto dto)
        {
            await _conferenceService.AddAsync(dto);
            return CreatedAtAction(nameof(Get), new {id = dto.Id}, null);
        }
        
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateAsync(Guid id, ConferenceDetailsDto dto)
        {
            dto.Id = id;
            await _conferenceService.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<HostDetailsDto>> DeleteAsync(Guid id)
        {
            await _conferenceService.DeleteAsync(id);
            return NoContent();
        }

    }
}