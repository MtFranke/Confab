using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confab.Modules.Conferences.Core.Entities;
using Confab.Modules.Conferences.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Conferences.Core.DAL.Repositories
{
    internal class HostRepository : IHostRepository
    {
        private readonly ConferencesDbContext _dbContext;
        private readonly DbSet<Host> _hosts;

        public HostRepository(ConferencesDbContext dbContext)
        {
            _dbContext = dbContext;
            _hosts = _dbContext.Host;
        }

        public async Task<Host> GetAsync(Guid id)
            => await _hosts
                .Include(x=>x.Conferences)
                .SingleOrDefaultAsync(x => x.Id == id);

        public async Task<IReadOnlyList<Host>> BrowseAsync()
            => await _hosts.ToListAsync();

        public async Task AddAsync(Host host)
        {
            await _hosts.AddAsync(host);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Host host)
        {
            _hosts.Update(host);
            await _dbContext.SaveChangesAsync();;
        }

        public async Task DeleteAsync(Host host)
        {
            _hosts.Remove(host);
            await _dbContext.SaveChangesAsync();
        }
    }
}