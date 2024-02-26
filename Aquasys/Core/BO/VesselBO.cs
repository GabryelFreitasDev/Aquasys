using Aquasys.Core.Data;
using Aquasys.Core.Entities;
using Aquasys.Core.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.Core.BO
{
    public class VesselBO : DatabaseContext, IAbstractBO<Vessel>
    {
        public VesselBO() { }

        public async Task<List<Vessel>> GetAllAsync()
        {
            var vessels = await GetAllAsync<Vessel>();
            return vessels.ToList();
        }

        public async Task<Vessel> GetByIdAsync(object primaryKey)
        {
            return await GetByIdAsync<Vessel>(primaryKey);
        }

        public async Task<List<Vessel>> GetFilteredAsync(Expression<Func<Vessel, bool>> predicate)
        {
            var vessels = await GetFilteredAsync<Vessel>(predicate);
            return vessels.ToList();
        }

        public async Task<bool> InsertAsync(Vessel item)
        {
            return await InsertAsync<Vessel>(item);
        }

        public async Task<bool> UpdateAsync(Vessel item)
        {
            return await UpdateAsync<Vessel>(item);
        }

        public async Task<bool> DeleteAsync(Vessel item)
        {
            return await DeleteAsync<Vessel>(item);
        }

        public async Task<bool> DeleteByIdAsync(object primaryKey)
        {
            return await DeleteByIdAsync<Vessel>(primaryKey);
        }
    }
}
