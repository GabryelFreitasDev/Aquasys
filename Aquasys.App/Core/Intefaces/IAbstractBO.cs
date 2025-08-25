using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.App.Core.Intefaces
{
    public interface IAbstractBO<TTable>
    {
        Task<List<TTable>> GetAllAsync();
        Task<TTable> GetByIdAsync(object primaryKey);
        Task<List<TTable>> GetFilteredAsync(Expression<Func<TTable, bool>> predicate);
        Task<bool> InsertAsync(TTable item);
        Task<bool> UpdateAsync(TTable item);
        Task<bool> DeleteAsync(TTable item);
        Task<bool> DeleteByIdAsync(object primaryKey);
        //Task<bool> SaveChangesAsync(TTable item);
    }
}
