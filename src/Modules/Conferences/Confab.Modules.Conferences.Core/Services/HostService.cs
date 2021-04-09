using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confab.Modules.Conferences.Core.DTO;
using Confab.Modules.Conferences.Core.Entities;
using Confab.Modules.Conferences.Core.Exceptions;
using Confab.Modules.Conferences.Core.Policies;
using Confab.Modules.Conferences.Core.Repositories;

namespace Confab.Modules.Conferences.Core.Services
{
    internal class HostService : IHostService
    {
        private readonly IHostRepository _hostRepository;
        private readonly IHostDeletionPolicy _hostDeletionPolicy;

        public HostService(IHostRepository hostRepository, IHostDeletionPolicy hostDeletionPolicy)
        {
            _hostRepository = hostRepository;
            _hostDeletionPolicy = hostDeletionPolicy;
        }

        public async Task AddAsync(HostDto dto)
        {
            dto.Id = Guid.NewGuid();
            await _hostRepository.AddAsync(new Host()
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description
            });
        }

        public async Task<HostDetailsDto> GetAsync(Guid id)
        {
            var host = await _hostRepository.GetAsync(id);
            if (host is null)
            {
                return null;
            }

            var dto = Map<HostDetailsDto>(host);
            dto.Conferences = host.Conferences.Select(x => new ConferenceDto()
            {
                Id = x.Id,
                HostId = x.HostId,
                Name = x.Name,
                From = x.From,
                To = x.To,
                Location = x.Location,
                Description = x.Description,
                LogoUrl = x.LogoUrl,
                Host = x.Host,
                ParticipantLimit = x.ParticipantLimit
            }).ToList();

            return dto;
        }

        public async Task<IReadOnlyList<HostDto>> BrowseAsync()
        {
            var hosts = await _hostRepository.BrowseAsync();

            return hosts.Select(Map<HostDto>).ToList();
        }

        public async Task UpdateAsync(HostDetailsDto dto)
        {
            var host = await _hostRepository.GetAsync(dto.Id);
            if (host is null)
            {
                throw new HostNotFoundException(dto.Id);
            }

            host.Name = dto.Name;
            host.Description = dto.Description;
            await _hostRepository.UpdateAsync(host);
        }

        public async Task DeleteAsync(Guid id)
        {
            var host = await _hostRepository.GetAsync(id);
            if (host is null)
            {
                throw new HostNotFoundException(id);
            }

            if (await _hostDeletionPolicy.CanDeleteAsync(host) is false)
            {
                throw new CannotDeleteHostException(id);
            }

            await _hostRepository.DeleteAsync(host);
        }

        private static T Map<T>(Host host) where T : HostDto, new()
            => new T
            {
                Id = host.Id,
                Name = host.Name,
                Description = host.Description
            };
    }
}