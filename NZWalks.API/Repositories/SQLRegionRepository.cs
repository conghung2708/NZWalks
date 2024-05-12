using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;
        public SQLRegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            _nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Region> CreateRegionAsync(Region region)
        {
            await _nZWalksDbContext.Regions.AddAsync(region);
            await _nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteRegionAsync(Guid Id)
        {
            var existingRegion = await _nZWalksDbContext.Regions.FirstOrDefaultAsync(u => u.Id == Id);
            if (existingRegion == null)
            {
                return null;
            }
            _nZWalksDbContext.Regions.Remove(existingRegion);  //Remove is not a async method
            await _nZWalksDbContext.SaveChangesAsync();
            return existingRegion;
        }

        public async Task<List<Region>> GetAllRegionAsync()
        {
            return await _nZWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetRegionByIdAsync(Guid id)
        {
            return await _nZWalksDbContext.Regions.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Region?> UpdateRegionAsync(Guid Id, Region region)
        {
            var existingRegion = await _nZWalksDbContext.Regions.FirstOrDefaultAsync(u => u.Id == Id);
            if (existingRegion == null)
            {
                return null;
            }

            existingRegion.Name = region.Name;
            existingRegion.Code = region.Code;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            await _nZWalksDbContext.SaveChangesAsync();

            return existingRegion;
        }
    }
}
