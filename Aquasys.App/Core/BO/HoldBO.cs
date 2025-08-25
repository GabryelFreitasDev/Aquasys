using Aquasys.App.Core.Data;
using Aquasys.App.Core.Entities;
using Aquasys.App.Core.Intefaces;
using System.Linq.Expressions;

namespace Aquasys.App.Core.BO
{
    internal class HoldBO : DatabaseContext, IAbstractBO<Hold>
    {
        public HoldBO() { }

        public async Task<List<Hold>> GetAllAsync()
        {
            var holds = await GetAllAsync<Hold>();
            return holds.ToList();
        }

        public async Task<Hold> GetByIdAsync(object primaryKey)
        {
            return await GetByIdAsync<Hold>(primaryKey);
        }

        public async Task<List<Hold>> GetFilteredAsync(Expression<Func<Hold, bool>> predicate)
        {
            var holds = await GetFilteredAsync<Hold>(predicate);
            return holds.ToList();
        }

        public async Task<bool> InsertAsync(Hold item)
        {
            return await InsertAsync<Hold>(item);
        }

        public async Task<bool> UpdateAsync(Hold item)
        {
            return await UpdateAsync<Hold>(item);
        }

        public async Task<bool> DeleteAsync(Hold item)
        {
            return await DeleteAsync<Hold>(item);
        }

        public async Task<bool> DeleteByIdAsync(object primaryKey)
        {
            return await DeleteByIdAsync<Hold>(primaryKey);
        }
    }
}
