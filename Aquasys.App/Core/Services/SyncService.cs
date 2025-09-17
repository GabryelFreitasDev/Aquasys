using Aquasys.App.Core.Data;

using Aquasys.Core.Interfaces;
using Newtonsoft.Json;
using System.Diagnostics;

using System.Net.Http.Json;


namespace Aquasys.Core.Sync
{
    public interface ISyncService
    {
        Task SynchronizeAsync();
    }

    public class SyncService : ISyncService
    {
        private readonly HttpClient _httpClient;
        private readonly IEnumerable<IBaseRepository> _repositories;
        private readonly Dictionary<string, Type> _typeRegistry;

        public SyncService(HttpClient httpClient, IServiceProvider serviceProvider)
        {
            _httpClient = httpClient;
            _typeRegistry = GetClientSideTypeRegistry();
            _repositories = _typeRegistry.Values.Select(type =>
                (IBaseRepository)serviceProvider.GetService(typeof(ILocalRepository<>).MakeGenericType(type))
            ).Where(repo => repo != null).ToList();
        }

        public async Task SynchronizeAsync()
        {
            Debug.WriteLine(">>> INICIANDO PROCESSO DE SINCRONIZAÇÃO COMPLETO <<<");
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine(">>> MODO OFFLINE. Sincronização adiada. <<<");
                return;
            }

            try
            {
                await PushDataAsync();
                await PullDataAsync();
                Debug.WriteLine(">>> SINCRONIZAÇÃO CONCLUÍDA COM SUCESSO <<<");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Falha na sincronização: {ex.Message} --- InnerException: {ex.InnerException?.Message}");
            }
        }

        private async Task PushDataAsync()
        {
            Debug.WriteLine("Iniciando fase de PUSH...");
            var pushRequest = new PushRequestDto();

            foreach (IBaseRepository repo in _repositories)
            {
                var unsyncedItems = await repo.GetUnsyncedAsync();
                if (unsyncedItems.Any())
                {
                    var entityName = repo.GetEntityType().Name;
                    pushRequest.Entities[entityName] = unsyncedItems.Cast<object>().ToList();
                }
            }

            if (!pushRequest.Entities.Any())
            {
                Debug.WriteLine("PUSH: Nada para enviar.");
                return;
            }

            var response = await _httpClient.PostAsJsonAsync("/api/sync/push", pushRequest);
            response.EnsureSuccessStatusCode();

            var repoMap = _repositories.ToDictionary(r => r.GetEntityType().Name);
            foreach (var entry in pushRequest.Entities)
            {
                if (repoMap.TryGetValue(entry.Key, out var repo))
                {
                    var globalIds = entry.Value.Select(e => ((SyncableEntity)e).GlobalId).ToList();
                    await repo.MarkAsSyncedAsync(globalIds);
                }
            }
            Debug.WriteLine("Fase de PUSH concluída.");
        }

        private async Task PullDataAsync()
        {
            Debug.WriteLine("Iniciando fase de PULL...");
            var lastSyncTimestamp = Preferences.Get("last_sync_timestamp", DateTime.MinValue);
            var lastSyncUtc = lastSyncTimestamp.ToUniversalTime().ToString("o");

            var response = await _httpClient.GetAsync($"/api/sync/pull?since={lastSyncUtc}");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var pullResponse = JsonConvert.DeserializeObject<PullResponseDto>(jsonResponse);

            if (pullResponse == null || !pullResponse.Entities.Any())
            {
                Debug.WriteLine("PULL: Nenhuma alteração no servidor.");
                Preferences.Set("last_sync_timestamp", pullResponse?.ServerTimestamp ?? DateTime.UtcNow);
                return;
            }

            // --- INÍCIO DA CORREÇÃO ---
            // Cria um dicionário para encontrar o repositório certo de forma eficiente
            var repoMap = _repositories.ToDictionary(r => r.GetEntityType().Name);

            foreach (var entry in pullResponse.Entities)
            {
                // Encontra o tipo da entidade e o repositório correspondente no dicionário
                if (_typeRegistry.TryGetValue(entry.Key, out var entityType) && repoMap.TryGetValue(entry.Key, out var repo))
                {
                    foreach (var item in entry.Value)
                    {
                        // Desserializa o item para o tipo correto e o salva no banco local
                        var typedItem = (JsonConvert.DeserializeObject(item.ToString(), entityType) as SyncableEntity);
                        if (typedItem != null)
                        {
                            await repo.UpsertAsync(typedItem);
                        }
                    }
                }
            }
            // --- FIM DA CORREÇÃO ---

            Preferences.Set("last_sync_timestamp", pullResponse.ServerTimestamp);
            Debug.WriteLine("Fase de PULL concluída.");
        }

        private Dictionary<string, Type> GetClientSideTypeRegistry()
        {
            return typeof(SyncableEntity).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(SyncableEntity)))
                .ToDictionary(t => t.Name, t => t);
        }
    }

    // A classe RepositoryExtensions e o método GetRepositories() foram removidos
    // pois não são mais necessários com a abordagem fortemente tipada.
}