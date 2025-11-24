using Aquasys.Core;
using Aquasys.Core.Sync;
using Aquasys.WebApi.Data;
using Aquasys.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Aquasys.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SyncTypeRegistry _types;

        private static readonly MethodInfo? SetMethod =
            typeof(DbContext).GetMethod(nameof(DbContext.Set), Type.EmptyTypes);

        public SyncController(AppDbContext context, SyncTypeRegistry registry)
        {
            _context = context;
            _types = registry;
        }

        [HttpPost("push")]
        public async Task<IActionResult> Push([FromBody] PushRequestDto dto)
        {
            if (dto == null || dto.Entities == null || dto.Entities.Count == 0)
                return Ok("Nada a sincronizar.");

            // Garante que pais venham antes de filhos, mas não mexe em IDs
            var processingOrder = GetProcessingOrder(dto.Entities.Keys.ToList());

            await using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var typeName in processingOrder)
                {
                    if (!dto.Entities.TryGetValue(typeName, out var arr) || arr.Count == 0)
                        continue;

                    var clr = _types.GetType(typeName);
                    if (clr == null) continue;

                    var dbSet = GetDbSet(clr);

                    var list = arr
                        .Select(o => JsonConvert.DeserializeObject(((JObject)o).ToString(), clr) as SyncableEntity)
                        .Where(e => e != null)
                        .ToList()!;

                    foreach (var e in list)
                        NormalizeUtc(e);

                    var gids = list.Select(e => e.GlobalId).ToList();
                    var existing = await dbSet.Where(e => gids.Contains(e.GlobalId)).ToListAsync();
                    var existingMap = existing.ToDictionary(e => e.GlobalId);

                    var toInsert = new List<SyncableEntity>();

                    foreach (var incoming in list)
                    {
                        // JÁ EXISTE no servidor → UPDATE (Last Write Wins)
                        if (existingMap.TryGetValue(incoming.GlobalId, out var current))
                        {
                            if (incoming.LastModifiedAt > current.LastModifiedAt)
                            {
                                CopyNonKeys(incoming, current);
                            }
                        }
                        else
                        {
                            // NÃO EXISTE → INSERT com o ID que veio do app (se tiver)
                            // NENHUM reset de PK ou FK aqui
                            toInsert.Add(incoming);
                        }
                    }

                    if (toInsert.Count > 0)
                        await _context.AddRangeAsync(toInsert);

                    await _context.SaveChangesAsync();

                    // Opcional: ajustar as sequences do Postgres para não ficarem para trás
                    await SyncIdentitySequenceFor(clr);
                }

                await tx.CommitAsync();
                return Ok(new { Status = "OK" });
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("pull")]
        public async Task<IActionResult> Pull(DateTime? since = null)
        {
            DateTime sinceUtc;

            if (since.HasValue)
            {
                sinceUtc = DateTime.SpecifyKind(since.Value, DateTimeKind.Utc);
            }
            else
            {
                sinceUtc = DateTime.UnixEpoch;
            }

            var result = new PullResponseDto
            {
                ServerTimestamp = DateTime.UtcNow
            };

            foreach (var kv in _types.GetAllTypes())
            {
                var clr = kv.Value;
                var name = kv.Key;

                var set = GetDbSet(clr);
                if (set == null) continue;

                var changed = await set.AsNoTracking()
                    .Where(e => e.LastModifiedAt > sinceUtc)
                    .ToListAsync();

                if (changed.Any())
                {
                    result.Entities[name] = changed.Cast<object>().ToList();
                }
            }

            return Ok(result);
        }

        // --------------------------------------------------------------
        // HELPERS
        // --------------------------------------------------------------

        private IQueryable<SyncableEntity> GetDbSet(Type clr)
        {
            var gm = SetMethod!.MakeGenericMethod(clr);
            return (IQueryable<SyncableEntity>)gm.Invoke(_context, null)!;
        }

        private static void NormalizeUtc(SyncableEntity e)
        {
            foreach (var p in e.GetType().GetProperties())
            {
                if (p.PropertyType == typeof(DateTime))
                {
                    var dt = (DateTime)p.GetValue(e)!;
                    p.SetValue(e, dt.ToUniversalTime());
                }
                else if (p.PropertyType == typeof(DateTime?))
                {
                    var dt = (DateTime?)p.GetValue(e);
                    if (dt.HasValue) p.SetValue(e, dt.Value.ToUniversalTime());
                }
            }
        }

        private static void CopyNonKeys(object src, object dst)
        {
            var type = src.GetType();
            foreach (var p in type.GetProperties())
            {
                if (!p.CanWrite) continue;
                if (p.IsDefined(typeof(KeyAttribute), true)) continue;
                if (!p.PropertyType.IsValueType && p.PropertyType != typeof(string)) continue;

                p.SetValue(dst, p.GetValue(src));
            }
        }

        private static long? GetKeyValue(object e)
        {
            var pk = e.GetType().GetProperties()
                .FirstOrDefault(p => p.IsDefined(typeof(KeyAttribute), true));
            if (pk == null) return null;

            var v = pk.GetValue(e);
            return v is long l ? l : null;
        }

        // ---------- ORDEM AUTOMÁTICA POR FKs ----------

        private List<string> GetProcessingOrder(List<string> typeNamesInPayload)
        {
            var allTypes = _types.GetAllTypes()
                       .ToDictionary(kv => kv.Key, kv => kv.Value); 
            var payloadTypes = typeNamesInPayload
                .Where(n => allTypes.ContainsKey(n))
                .Select(n => allTypes[n])
                .ToHashSet();

            if (payloadTypes.Count == 0)
                return new List<string>(); // nada pra ordenar

            // 2) Monta o grafo de dependências usando o modelo do EF Core
            var graph = new Dictionary<Type, HashSet<Type>>();  // nó -> depende de
            var reverse = new Dictionary<Type, HashSet<Type>>(); // nó -> filhos

            foreach (var et in _context.Model.GetEntityTypes())
            {
                var clr = et.ClrType;

                // só tipos de sync presentes no payload
                if (!payloadTypes.Contains(clr))
                    continue;
                if (!typeof(SyncableEntity).IsAssignableFrom(clr))
                    continue;

                if (!graph.ContainsKey(clr))
                    graph[clr] = new HashSet<Type>();
                if (!reverse.ContainsKey(clr))
                    reverse[clr] = new HashSet<Type>();

                foreach (var fk in et.GetForeignKeys())
                {
                    var principalClr = fk.PrincipalEntityType.ClrType;

                    // só considera FKs entre tipos de sync presentes
                    if (!payloadTypes.Contains(principalClr))
                        continue;
                    if (!typeof(SyncableEntity).IsAssignableFrom(principalClr))
                        continue;

                    // clr DEPENDE de principalClr (ou seja, principal tem que vir antes)
                    graph[clr].Add(principalClr);

                    if (!reverse.ContainsKey(principalClr))
                        reverse[principalClr] = new HashSet<Type>();
                    reverse[principalClr].Add(clr);
                }
            }

            // 3) Topological sort (Kahn)
            var noDeps = new Queue<Type>(
                graph.Where(kv => kv.Value.Count == 0).Select(kv => kv.Key)
            );

            var orderedTypes = new List<Type>();
            var deps = graph.ToDictionary(
                kv => kv.Key,
                kv => new HashSet<Type>(kv.Value)
            );

            while (noDeps.Count > 0)
            {
                var n = noDeps.Dequeue();
                orderedTypes.Add(n);

                if (!reverse.TryGetValue(n, out var children)) continue;

                foreach (var m in children.ToList())
                {
                    if (!deps.TryGetValue(m, out var dm)) continue;

                    dm.Remove(n);
                    if (dm.Count == 0)
                        noDeps.Enqueue(m);
                }
            }

            // 4) fallback se sobrar nós (ciclo ou info faltando)
            if (orderedTypes.Count != graph.Count)
            {
                var missing = graph.Keys.Except(orderedTypes).ToList();
                orderedTypes.AddRange(missing);
            }

            // 5) Converte Type -> string (nome do SyncTypeRegistry)
            var result = new List<string>();

            foreach (var t in orderedTypes)
            {
                var name = allTypes.FirstOrDefault(kv => kv.Value == t).Key;
                if (!string.IsNullOrEmpty(name) && typeNamesInPayload.Contains(name))
                    result.Add(name);
            }

            // garante que nenhum tipo do payload fique de fora
            foreach (var n in typeNamesInPayload)
            {
                if (!result.Contains(n))
                    result.Add(n);
            }

            return result;
        }

        private async Task SyncIdentitySequenceFor(Type clr)
        {
            var entity = _context.Model.FindEntityType(clr);
            var pk = entity?.FindPrimaryKey();
            if (pk == null) return;

            var pkProp = pk.Properties.FirstOrDefault();
            if (pkProp == null) return;

            // só se for PK numérica
            if (pkProp.ClrType != typeof(long) && pkProp.ClrType != typeof(int))
                return;

            var tableName = entity.GetTableName();
            var columnName = pkProp.GetColumnName(StoreObjectIdentifier.Table(tableName!, null));

            var sql = $@"
                DO $$
                DECLARE seq text;
                BEGIN
                    SELECT pg_get_serial_sequence('{tableName}', '{columnName}') INTO seq;

                    IF seq IS NOT NULL THEN
                        PERFORM setval(
                            seq,
                            COALESCE((SELECT MAX(""{columnName}"") FROM ""{tableName}""), 1)
                        );
                    END IF;
                END $$;
                ";

            await _context.Database.ExecuteSqlRawAsync(sql);
        }
    }
}
