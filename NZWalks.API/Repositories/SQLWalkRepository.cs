using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;
        public SQLWalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            _nZWalksDbContext = nZWalksDbContext;
        }
        public async Task<Walk> CreateWalkAsync(Walk walk)
        {
            await _nZWalksDbContext.Walks.AddAsync(walk);
            await _nZWalksDbContext.SaveChangesAsync();
            return walk;

        }

        public async Task<List<Walk>> GetAllWalkAsync()
        {
            return await _nZWalksDbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetWalkByIdAsync(Guid id)
        {
            return await _nZWalksDbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
