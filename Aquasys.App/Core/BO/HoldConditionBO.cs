using Aquasys.App.Core.Data;
using Aquasys.App.Core.Entities;
using Aquasys.App.Core.Intefaces;
using System.Linq.Expressions;

namespace Aquasys.App.Core.BO
{
    internal class HoldConditionBO : DatabaseContext, IAbstractBO<HoldCondition>
    {
        public HoldConditionBO() { }

        public async Task<List<HoldCondition>> GetAllAsync()
        {
            var HoldConditions = await GetAllAsync<HoldCondition>();
            return HoldConditions.ToList();
        }

        public async Task<HoldCondition> GetByIdAsync(object primaryKey)
        {
            return await GetByIdAsync<HoldCondition>(primaryKey);
        }

        public async Task<List<HoldCondition>> GetFilteredAsync(Expression<Func<HoldCondition, bool>> predicate)
        {
            var HoldConditions = await GetFilteredAsync<HoldCondition>(predicate);
            return HoldConditions.ToList();
        }

        public async Task<bool> InsertAsync(HoldCondition item)
        {
            return await InsertAsync<HoldCondition>(item);
        }

        public async Task<bool> UpdateAsync(HoldCondition item)
        {
            return await UpdateAsync<HoldCondition>(item);
        }

        public async Task<bool> DeleteAsync(HoldCondition item)
        {
            return await DeleteAsync<HoldCondition>(item);
        }

        public async Task<bool> DeleteByIdAsync(object primaryKey)
        {
            return await DeleteByIdAsync<HoldCondition>(primaryKey);
        }
    }
}
