using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateWalkAsync(Walk walk);
        Task<List<Walk>> GetAllWalkAsync(string? filterOn = null, string? filterQuery = null);
        Task<Walk?> GetWalkByIdAsync(Guid id);
        Task<Walk?> UpdateWalkAsync(Guid id, Walk walk);
        Task<Walk?> DeleteAsync(Guid id);
    }
}
