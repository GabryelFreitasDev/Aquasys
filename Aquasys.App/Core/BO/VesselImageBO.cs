using Aquasys.App.Core.Data;
using Aquasys.App.Core.Entities;
using Aquasys.App.Core.Intefaces;
using System.Linq.Expressions;

namespace Aquasys.App.Core.BO
{
    public class VesselImageBO : DatabaseContext, IAbstractBO<VesselImage>
    {
        public VesselImageBO() { }

        public async Task<List<VesselImage>> GetAllAsync()
        {
            var VesselImages = await GetAllAsync<VesselImage>();
            return VesselImages.ToList();
        }

        public async Task<VesselImage> GetByIdAsync(object primaryKey)
        {
            return await GetByIdAsync<VesselImage>(primaryKey);
        }

        public async Task<List<VesselImage>> GetFilteredAsync(Expression<Func<VesselImage, bool>> predicate)
        {
            var VesselImages = await GetFilteredAsync<VesselImage>(predicate);
            return VesselImages.ToList();
        }

        public async Task<bool> InsertAsync(VesselImage item)
        {
            return await InsertAsync<VesselImage>(item);
        }

        public async Task<bool> UpdateAsync(VesselImage item)
        {
            return await UpdateAsync<VesselImage>(item);
        }

        public async Task<bool> DeleteAsync(VesselImage item)
        {
            return await DeleteAsync<VesselImage>(item);
        }

        public async Task<bool> DeleteByIdAsync(object primaryKey)
        {
            return await DeleteByIdAsync<VesselImage>(primaryKey);
        }
    }
}
