namespace Aquasys.Core.Interfaces
{
    public interface IBaseRepository
    {
        Type GetEntityType();
        Task<IEnumerable<SyncableEntity>> GetUnsyncedAsync();
        Task MarkAsSyncedAsync(List<Guid> globalIds);
        Task UpsertAsync(SyncableEntity item, bool fromServer = false);
    }
}
