using Aquasys.Core;
using Aquasys.Core.Sync;
using Aquasys.WebApi.Data;
using Aquasys.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Aquasys.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SyncTypeRegistry _typeRegistry;
        private static readonly MethodInfo? SetMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), Type.EmptyTypes);

        public SyncController(AppDbContext context, SyncTypeRegistry typeRegistry)
        {
            _context = context;
            _typeRegistry = typeRegistry;
        }

        [HttpPost("push")]
        public async Task<IActionResult> Push([FromBody] PushRequestDto pushRequest)
        {
            if (pushRequest == null || !pushRequest.Entities.Any())
            {
                return Ok("Nada para sincronizar.");
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var entry in pushRequest.Entities)
                {
                    var entityType = _typeRegistry.GetType(entry.Key);
                    if (entityType == null) continue;

                    var items = entry.Value
                        .Select(obj => (JsonConvert.DeserializeObject(obj.ToString(), entityType) as SyncableEntity))
                        .Where(item => item != null)
                        .ToList();

                    // AJUSTE 1: Correção do DateTimeKind para evitar o erro do PostgreSQL.
                    // Este bloco garante que todas as datas sejam salvas como UTC.
                    foreach (var item in items)
                    {
                        var dateTimeProperties = item.GetType().GetProperties()
                            .Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));

                        foreach (var prop in dateTimeProperties)
                        {
                            if (prop.GetValue(item) is DateTime dt && dt.Kind == DateTimeKind.Unspecified)
                            {
                                prop.SetValue(item, DateTime.SpecifyKind(dt, DateTimeKind.Utc));
                            }
                        }
                    }

                    // AJUSTE 3: Usando o método auxiliar para deixar o código mais limpo.
                    var dbSet = GetDbSetFor(entityType);
                    if (dbSet == null) continue;

                    var receivedGlobalIds = items.Select(i => i.GlobalId).ToList();
                    var existingEntities = await dbSet.Where(e => receivedGlobalIds.Contains(e.GlobalId)).ToListAsync();

                    // AJUSTE 2: Otimização de Performance.
                    // Converter para um dicionário torna a busca por um item existente instantânea,
                    // em vez de ter que percorrer a lista 'existingEntities' toda vez.
                    var existingEntitiesMap = existingEntities.ToDictionary(e => e.GlobalId);

                    var toAdd = new List<SyncableEntity>();

                    foreach (var item in items)
                    {
                        var primaryKeyProperty = item.GetType().GetProperties()
                            .FirstOrDefault(p => p.IsDefined(typeof(KeyAttribute), true));

                        // Se o item JÁ EXISTE no servidor (lógica de ATUALIZAÇÃO)
                        if (existingEntitiesMap.TryGetValue(item.GlobalId, out var existingEntity))
                        {
                            if (item.LastModifiedAt > existingEntity.LastModifiedAt)
                            {
                                var propertiesToUpdate = item.GetType().GetProperties()
                                    .Where(p => p.Name != primaryKeyProperty?.Name);

                                foreach (var prop in propertiesToUpdate)
                                {
                                    // Pega o novo valor do objeto que veio do cliente.
                                    var newValue = prop.GetValue(item);
                                    // Define o novo valor na entidade que veio do banco de dados.
                                    prop.SetValue(existingEntity, newValue);
                                }
                            }
                        }
                        else // Se o item é NOVO (lógica de INSERÇÃO)
                        {
                            if (primaryKeyProperty != null)
                            {
                                var pkType = primaryKeyProperty.PropertyType;
                                primaryKeyProperty.SetValue(item, Activator.CreateInstance(pkType));
                            }
                            toAdd.Add(item);
                        }
                    }

                    // Usar AddRange (síncrono) é geralmente mais seguro com o ChangeTracker do EF Core
                    if (toAdd.Any()) _context.AddRange(toAdd);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new { Status = "Sincronizado com sucesso" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Ocorreu um erro durante a sincronização: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpGet("pull")]
        public async Task<IActionResult> Pull([FromQuery] DateTime since)
        {
            var response = new PullResponseDto
            {
                ServerTimestamp = DateTime.UtcNow,
                Entities = new Dictionary<string, List<object>>()
            };

            // Garantimos que a data 'since' também seja UTC para a comparação no banco.
            var sinceUtc = DateTime.SpecifyKind(since, DateTimeKind.Utc);

            foreach (var entityTypeEntry in _typeRegistry.GetAllTypes())
            {
                // AJUSTE 3: Usando o método auxiliar aqui também.
                var dbSet = GetDbSetFor(entityTypeEntry.Value);
                if (dbSet == null) continue;

                var changedEntities = await dbSet.AsNoTracking()
                                                 .Where(e => e.LastModifiedAt > sinceUtc)
                                                 .ToListAsync();

                if (changedEntities.Any())
                {
                    response.Entities[entityTypeEntry.Key] = changedEntities.Cast<object>().ToList();
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// AJUSTE 3: Método auxiliar para obter um DbSet de forma dinâmica, evitando repetição de código.
        /// </summary>
        private IQueryable<SyncableEntity>? GetDbSetFor(Type entityType)
        {
            var genericSetMethod = SetMethod?.MakeGenericMethod(entityType);
            if (genericSetMethod == null) return null;

            return (IQueryable<SyncableEntity>?)genericSetMethod.Invoke(_context, null);
        }
    }
}