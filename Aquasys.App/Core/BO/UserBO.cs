using Aquasys.App.Core.Data;
using Aquasys.App.Core.Entities;
using Aquasys.App.Core.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.App.Core.BO
{
    public class UserBO : DatabaseContext, IAbstractBO<User>
    {
        public UserBO() { }

        public async Task<List<User>> GetAllAsync()
        {
            var users = await GetAllAsync<User>();
            return users.ToList();
        }

        public async Task<User> GetByIdAsync(object primaryKey)
        {
            return await GetByIdAsync<User>(primaryKey);
        }

        public async Task<List<User>> GetFilteredAsync(Expression<Func<User, bool>> predicate)
        {
            var users = await GetFilteredAsync<User>(predicate);
            return users.ToList();
        }

        public async Task<bool> InsertAsync(User item)
        {
            return await InsertAsync<User>(item);
        }

        public async Task<bool> UpdateAsync(User item)
        {
            return await UpdateAsync<User>(item);
        }

        public async Task<bool> DeleteAsync(User item)
        {
            return await DeleteAsync<User>(item);
        }

        public async Task<bool> DeleteByIdAsync(object primaryKey)
        {
            return await DeleteByIdAsync<User>(primaryKey);
        }
    }
}
