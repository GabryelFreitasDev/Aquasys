using Aquasys.Core.Data;
using Aquasys.Core.Entities;
using Aquasys.Core.Intefaces;
using System.Linq.Expressions;

namespace Aquasys.Core.BO
{
    internal class HoldInspectionBO : DatabaseContext, IAbstractBO<HoldInspection>
    {
        public HoldInspectionBO() { }

        public async Task<List<HoldInspection>> GetAllAsync()
        {
            var HoldInspections = await GetAllAsync<HoldInspection>();
            return HoldInspections.ToList();
        }

        public async Task<HoldInspection> GetByIdAsync(object primaryKey)
        {
            return await GetByIdAsync<HoldInspection>(primaryKey);
        }

        public async Task<List<HoldInspection>> GetFilteredAsync(Expression<Func<HoldInspection, bool>> predicate)
        {
            var HoldInspections = await GetFilteredAsync<HoldInspection>(predicate);
            return HoldInspections.ToList();
        }

        public async Task<bool> InsertAsync(HoldInspection item)
        {
            return await InsertAsync<HoldInspection>(item);
        }

        public async Task<bool> UpdateAsync(HoldInspection item)
        {
            return await UpdateAsync<HoldInspection>(item);
        }

        public async Task<bool> DeleteAsync(HoldInspection item)
        {
            return await DeleteAsync<HoldInspection>(item);
        }

        public async Task<bool> DeleteByIdAsync(object primaryKey)
        {
            return await DeleteByIdAsync<HoldInspection>(primaryKey);
        }
    }
}
