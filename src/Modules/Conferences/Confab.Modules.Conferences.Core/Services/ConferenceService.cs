using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confab.Modules.Conferences.Core.DTO;
using Confab.Modules.Conferences.Core.Entities;
using Confab.Modules.Conferences.Core.Exceptions;
using Confab.Modules.Conferences.Core.Policies;
using Confab.Modules.Conferences.Core.Repositories;
using Confab.Shared.Abstractions.Messaging;
using ConferenceCreated = Confab.Modules.Conferences.Core.Events.ConferenceCreated;

namespace Confab.Modules.Conferences.Core.Services
{
    internal class ConferenceService : IConferenceService
    {
        private readonly IConferenceRepository _conferenceRepository;
        private readonly IHostRepository _hostRepository;
        private readonly IConferenceDeletionPolicy _conferenceDeletionPolicy;
        private readonly IMessageBroker _messageBroker;

        public ConferenceService(
            IConferenceRepository conferenceRepository,
            IHostRepository hostRepository,
            IConferenceDeletionPolicy conferenceDeletionPolicy, IMessageBroker messageBroker)
        {
            _conferenceRepository = conferenceRepository;
            _hostRepository = hostRepository;
            _conferenceDeletionPolicy = conferenceDeletionPolicy;
            _messageBroker = messageBroker;
        }

        public async Task AddAsync(ConferenceDetailsDto dto)
        {
            if (await _hostRepository.GetAsync(dto.HostId) is null)
            {
                throw new HostNotFoundException(dto.Id);
            }

            dto.Id = Guid.NewGuid();
            var conference = new Conference()
            {
                Id = dto.Id,
                HostId = dto.HostId,
                Name = dto.Name,
                Description = dto.Description,
                From = dto.From,
                To = dto.To,
                Location = dto.Location,
                LogoUrl = dto.LogoUrl,
                ParticipantLimit = dto.ParticipantLimit
            };
            await _conferenceRepository.AddAsync(conference);

            await _messageBroker.PublishAsync(new ConferenceCreated(conference.Id, conference.Name,
                conference.ParticipantLimit, conference.From, conference.To));
        }

        public async Task<ConferenceDetailsDto> GetAsync(Guid id)
        {
            var conference = await _conferenceRepository.GetAsync(id);
            if (conference is null)
                return null;
            var dto = Map<ConferenceDetailsDto>(conference);
            dto.Description = conference.Description;
            return dto;
        }

        public async Task<IReadOnlyList<ConferenceDto>> BrowseAsync()
        {
            var conferences = await _conferenceRepository.BrowseAsync();
            return conferences.Select(Map<ConferenceDto>).ToList();
        }

        public async Task UpdateAsync(ConferenceDetailsDto dto)
        {
            var conference = await _conferenceRepository.GetAsync(dto.Id);
            if (conference is null)
            {
                throw new ConferenceNotFoundException(dto.Id);
            }

            conference.Name = dto.Name;
            conference.Description = dto.Description;
            conference.From = dto.From;
            conference.To = dto.To;
            conference.LogoUrl = dto.LogoUrl;
            conference.ParticipantLimit = dto.ParticipantLimit;
            conference.Location = dto.Location;

            await _conferenceRepository.UpdateAsync(conference);
        }

        public async Task DeleteAsync(Guid id)
        {
            var conference = await _conferenceRepository.GetAsync(id);
            if (conference is null)
            {
                throw new ConferenceNotFoundException(id);
            }

            if (await _conferenceDeletionPolicy.CanDeleteAsync(conference) is false)
            {
                throw new ConferenceNotFoundException(id);
            }

            await _conferenceRepository.DeleteAsync(conference);
        }


        private static T Map<T>(Conference conference) where T : ConferenceDto, new()
            => new T()
            {
                Id = conference.Id,
                HostId = conference.HostId,
                Description = conference.Description,
                From = conference.From,
                To = conference.To,
                Location = conference.Location,
                Name = conference.Name,
                LogoUrl = conference.LogoUrl,
                ParticipantLimit = conference.ParticipantLimit,
                Host = conference.Host
            };
    }
}