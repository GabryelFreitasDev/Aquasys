using Aquasys.Core.Data;
using Aquasys.Core.Entities;
using Aquasys.Core.Intefaces;
using System.Linq.Expressions;

namespace Aquasys.Core.BO
{
    public class HoldImageBO : DatabaseContext, IAbstractBO<HoldImage>
    {
        public HoldImageBO() { }

        public async Task<List<HoldImage>> GetAllAsync()
        {
            var HoldImages = await GetAllAsync<HoldImage>();
            return HoldImages.ToList();
        }

        public async Task<HoldImage> GetByIdAsync(object primaryKey)
        {
            return await GetByIdAsync<HoldImage>(primaryKey);
        }

        public async Task<List<HoldImage>> GetFilteredAsync(Expression<Func<HoldImage, bool>> predicate)
        {
            var HoldImages = await GetFilteredAsync<HoldImage>(predicate);
            return HoldImages.ToList();
        }

        public async Task<bool> InsertAsync(HoldImage item)
        {
            return await InsertAsync<HoldImage>(item);
        }

        public async Task<bool> UpdateAsync(HoldImage item)
        {
            return await UpdateAsync<HoldImage>(item);
        }

        public async Task<bool> DeleteAsync(HoldImage item)
        {
            return await DeleteAsync<HoldImage>(item);
        }

        public async Task<bool> DeleteByIdAsync(object primaryKey)
        {
            return await DeleteByIdAsync<HoldImage>(primaryKey);
        }
    }
}
