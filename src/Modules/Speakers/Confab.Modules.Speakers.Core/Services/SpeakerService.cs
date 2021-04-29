using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confab.Modules.Speakers.Core.DTO;
using Confab.Modules.Speakers.Core.Events;
using Confab.Modules.Speakers.Core.Exceptions;
using Confab.Modules.Speakers.Core.Mappings;
using Confab.Modules.Speakers.Core.Repositories;
using Confab.Shared.Abstractions.Messaging;

namespace Confab.Modules.Speakers.Core.Services
{
    internal class SpeakerService : ISpeakerService
    {
        private readonly ISpeakerRepository _speakerRepository;
        private readonly IMessageBroker _messageBroker;

        public SpeakerService(ISpeakerRepository speakerRepository, IMessageBroker messageBroker)
        {
            _speakerRepository = speakerRepository;
            _messageBroker = messageBroker;
        }

        public async Task<IEnumerable<SpeakerDto>> BrowseAsync()
        {
            var speakers = await _speakerRepository.BrowseAsync();
            return speakers?.Select(e => e.AsDto());
        }

        public async Task<SpeakerDto> GetAsync(Guid speakerId)
        {
            var entity = await _speakerRepository.GetAsync(speakerId);
            return entity?.AsDto();
        }

        public async Task CreateAsync(SpeakerDto speaker)
        {
            var alreadyExists = await _speakerRepository.ExistsAsync(speaker.Id);
            if (alreadyExists)
            {
                throw new SpeakerAlreadyExistsException(speaker.Id);
            }

            await _speakerRepository.AddAsync(speaker.AsEntity());
            await _messageBroker.PublishAsync(new SpeakerCreated(speaker.Id, speaker.FullName));
        }

        public async Task UpdateAsync(SpeakerDto speaker)
        {
            var exists = await _speakerRepository.ExistsAsync(speaker.Id);

            if (!exists)
            {
                throw new SpeakerNotFoundException(speaker.Id);
            }

            await _speakerRepository.UpdateAsync(speaker.AsEntity());
        }
    }
}