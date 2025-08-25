using Aquasys.App.Core.Data;
using Aquasys.App.Core.Entities;
using Aquasys.App.Core.Intefaces;
using System.Linq.Expressions;

namespace Aquasys.App.Core.BO
{
    internal class InspectionBO : DatabaseContext, IAbstractBO<Inspection>
    {
        public InspectionBO() { }

        public async Task<List<Inspection>> GetAllAsync()
        {
            var Inspections = await GetAllAsync<Inspection>();
            return Inspections.ToList();
        }

        public async Task<Inspection> GetByIdAsync(object primaryKey)
        {
            return await GetByIdAsync<Inspection>(primaryKey);
        }

        public async Task<List<Inspection>> GetFilteredAsync(Expression<Func<Inspection, bool>> predicate)
        {
            var Inspections = await GetFilteredAsync<Inspection>(predicate);
            return Inspections.ToList();
        }

        public async Task<bool> InsertAsync(Inspection item)
        {
            return await InsertAsync<Inspection>(item);
        }

        public async Task<bool> UpdateAsync(Inspection item)
        {
            return await UpdateAsync<Inspection>(item);
        }

        public async Task<bool> DeleteAsync(Inspection item)
        {
            return await DeleteAsync<Inspection>(item);
        }

        public async Task<bool> DeleteByIdAsync(object primaryKey)
        {
            return await DeleteByIdAsync<Inspection>(primaryKey);
        }
    }
}
