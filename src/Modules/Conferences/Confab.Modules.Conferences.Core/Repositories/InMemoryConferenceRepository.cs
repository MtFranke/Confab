using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confab.Modules.Conferences.Core.Entities;

namespace Confab.Modules.Conferences.Core.Repositories
{
    internal class InMemoryConferenceRepository : IConferenceRepository
    {
        //Not thread safe, use Concurrent collections
        private readonly List<Conference> _conference = new();
        public Task<Conference> GetAsync(Guid id) => Task.FromResult<Conference>(_conference.SingleOrDefault(x => x.Id == id));

        public async Task<IReadOnlyList<Conference>> BrowseAsync()
        {
            await Task.CompletedTask;
            return _conference;
        }

        public Task AddAsync(Conference host)
        {
            _conference.Add(host);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Conference host) => Task.CompletedTask;

        public Task DeleteAsync(Conference host)
        {
            _conference.Remove(host);
            return Task.CompletedTask;
        }
    }
}