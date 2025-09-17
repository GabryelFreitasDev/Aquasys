using Aquasys.Core;
using Aquasys.Core.Sync;
using Aquasys.WebApi.Data;
using Aquasys.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

            var processingOrder = new List<string> {
                "User", "TypeVessel", "Vessel", "Inspection", "Hold",
                "VesselImage", "HoldInspection", "HoldImage", "HoldCargo", "HoldCondition"
            };

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var globalIdToServerIdMap = new Dictionary<Guid, long>();

                // 3. Processa cada tipo de entidade NA ORDEM definida.
                foreach (var entityName in processingOrder)
                {
                    if (!pushRequest.Entities.TryGetValue(entityName, out var entityObjects) || !entityObjects.Any())
                        continue;

                    var entityType = _typeRegistry.GetType(entityName);
                    if (entityType == null) continue;

                    var items = entityObjects
                        .Select(obj => (JsonConvert.DeserializeObject(obj.ToString(), entityType) as SyncableEntity))
                        .Where(item => item != null).ToList();

                    if (!items.Any()) continue;

                    var dbSet = GetDbSetFor(entityType);
                    var receivedGlobalIds = items.Select(i => i.GlobalId).ToList();
                    var existingEntities = await dbSet.Where(e => receivedGlobalIds.Contains(e.GlobalId)).ToListAsync();
                    var existingEntitiesMap = existingEntities.ToDictionary(e => e.GlobalId);
                    var toAdd = new List<SyncableEntity>();

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
                        var primaryKeyProperty = item.GetType().GetProperties()
                            .FirstOrDefault(p => p.IsDefined(typeof(KeyAttribute), true));

                        var foreignKeyProperties = item.GetType().GetProperties()
                            .Where(p => p.IsDefined(typeof(ForeignKeyAttribute), true));

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
                            //if (primaryKeyProperty != null)
                            //{
                            //    var pkType = primaryKeyProperty.PropertyType;
                            //    primaryKeyProperty.SetValue(item, Activator.CreateInstance(pkType));
                            //}
                            toAdd.Add(item);
                        }
                    }

                    if (toAdd.Any()) await _context.AddRangeAsync(toAdd);

                    // 6. SALVAR EM ETAPAS: Salva as mudanças deste lote de entidades (ex: todos os Vessels).
                    // Isso gera os IDs numéricos para os pais.
                    await _context.SaveChangesAsync();

                    // 7. POPULAR O MAPA: Após salvar, guardamos os novos IDs gerados.
                    foreach (var newItem in toAdd)
                    {
                        var pkProp = newItem.GetType().GetProperties().FirstOrDefault(p => p.IsDefined(typeof(KeyAttribute), true));
                        if (pkProp != null)
                        {
                            globalIdToServerIdMap[newItem.GlobalId] = (long)pkProp.GetValue(newItem);
                        }
                    }
                }

                //await _context.SaveChangesAsync();
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