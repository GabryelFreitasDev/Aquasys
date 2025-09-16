using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.Core.Interfaces
{
    public interface IBaseRepository
    {
        Type GetEntityType();
        Task<IEnumerable<SyncableEntity>> GetUnsyncedAsync();
        Task MarkAsSyncedAsync(List<Guid> globalIds);
        Task UpsertAsync(SyncableEntity item);
    }
}
