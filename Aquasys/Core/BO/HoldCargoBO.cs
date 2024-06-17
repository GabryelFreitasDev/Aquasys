using Aquasys.Core.Data;
using Aquasys.Core.Entities;
using Aquasys.Core.Intefaces;
using System.Linq.Expressions;

namespace Aquasys.Core.BO
{
    internal class HoldCargoBO : DatabaseContext, IAbstractBO<HoldCargo>
    {
        public HoldCargoBO() { }

        public async Task<List<HoldCargo>> GetAllAsync()
        {
            var HoldCargos = await GetAllAsync<HoldCargo>();
            return HoldCargos.ToList();
        }

        public async Task<HoldCargo> GetByIdAsync(object primaryKey)
        {
            return await GetByIdAsync<HoldCargo>(primaryKey);
        }

        public async Task<List<HoldCargo>> GetFilteredAsync(Expression<Func<HoldCargo, bool>> predicate)
        {
            var HoldCargos = await GetFilteredAsync<HoldCargo>(predicate);
            return HoldCargos.ToList();
        }

        public async Task<bool> InsertAsync(HoldCargo item)
        {
            return await InsertAsync<HoldCargo>(item);
        }

        public async Task<bool> UpdateAsync(HoldCargo item)
        {
            return await UpdateAsync<HoldCargo>(item);
        }

        public async Task<bool> DeleteAsync(HoldCargo item)
        {
            return await DeleteAsync<HoldCargo>(item);
        }

        public async Task<bool> DeleteByIdAsync(object primaryKey)
        {
            return await DeleteByIdAsync<HoldCargo>(primaryKey);
        }
    }
}
