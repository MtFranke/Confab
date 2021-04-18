using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confab.Modules.Speakers.Core.Entities;
using Confab.Modules.Speakers.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Confab.Modules.Speakers.Core.DAL.Repositories
{
    public class SpeakerRepository : ISpeakerRepository
    {
        private readonly SpeakerDbContext _context;
        private readonly DbSet<Speaker> _speakers;

        public SpeakerRepository(SpeakerDbContext context)
        {
            _context = context;
            _speakers = context.Speakers;
        }
        
        public async Task<IReadOnlyList<Speaker>> BrowseAsync()
        {
            return await _speakers.ToListAsync();
        }

        public async Task<Speaker> GetAsync(Guid id)
        {
            return await _context.Speakers.Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Speakers.AnyAsync(x=>x.Id==id);
        }

        public async Task AddAsync(Speaker speaker)
        {
            await _speakers.AddAsync(speaker);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Speaker speaker)
        {
            _speakers.Update(speaker);
            await _context.SaveChangesAsync();
        }
    }
}