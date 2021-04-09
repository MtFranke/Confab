using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Confab.Modules.Conferences.Core.DTO;
using Confab.Modules.Conferences.Core.Repositories;

namespace Confab.Modules.Conferences.Core.Services
{
    internal class ConferenceService : IConferenceService
    {
        private readonly IConferenceRepository _conferenceRepository;

        public ConferenceService(IConferenceRepository conferenceRepository)
        {
            _conferenceRepository = conferenceRepository;
        }
        
        public Task AddAsync(ConferenceDetailsDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ConferenceDetailsDto> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ConferenceDto>> BrowseAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ConferenceDetailsDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}