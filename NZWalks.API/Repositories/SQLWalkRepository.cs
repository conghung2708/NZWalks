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

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk = await _nZWalksDbContext.Walks.FirstOrDefaultAsync(u => u.Id == id);


            if (existingWalk == null) {
                return null;
            }

            _nZWalksDbContext.Remove(existingWalk);
            _nZWalksDbContext.SaveChangesAsync();

            return existingWalk;
        }

        public async Task<List<Walk>> GetAllWalkAsync(string? filterOn = null, string? filterQuery = null)
        {
            var walks = _nZWalksDbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //Filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(u => u.Name.Contains(filterQuery));
                }
            }
           

            return await walks.ToListAsync();
            //return await _nZWalksDbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetWalkByIdAsync(Guid id)
        {
            return await _nZWalksDbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Walk?> UpdateWalkAsync(Guid id, Walk walk)
        {
            var existingWalk = _nZWalksDbContext.Walks.FirstOrDefault(walk => walk.Id == id);
            if (existingWalk == null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;

            await _nZWalksDbContext.SaveChangesAsync();

            return existingWalk;
        }
    }
}
