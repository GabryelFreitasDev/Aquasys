using Aquasys.Core.Entities;
using Aquasys.WebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aquasys.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")] // Rota: /api/vessels
public class VesselsController : ControllerBase
{
    private readonly AppDbContext _context;

    public VesselsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vessel>>> GetVessels()
    {
        try
        {
            // Busca todas as embarcações no banco de dados
            var vessels = await _context.Vessels.ToListAsync();
            return Ok(vessels); // Retorna a lista com status 200 OK
        }
        catch (Exception ex)
        {
            // Em caso de erro no banco, retorna um erro 500
            Console.WriteLine($"Erro ao buscar Vessels: {ex}");
            return StatusCode(500, "Erro interno ao buscar dados das embarcações.");
        }
    }

    [HttpGet("{id:guid}")] // Rota: GET /api/Vessels/GUID_DA_EMBARCACAO
    public async Task<ActionResult<Vessel>> GetVesselById(Guid id, [FromQuery] bool includeHolds = false)
    {
        try
        {
            // Prepara a consulta para buscar o Vessel pelo GlobalId
            var query = _context.Vessels.Where(v => v.GlobalId == id);

            // Se o parâmetro 'includeHolds' for verdadeiro, inclui os Holds na consulta
            if (includeHolds)
            {
                query = query.Include(v => v.Holds); // Inclui a lista de Holds relacionada
            }

            // Executa a consulta
            var vessel = await query.FirstOrDefaultAsync();

            if (vessel == null)
            {
                return NotFound($"Embarcação com GlobalId {id} não encontrada.");
            }

            // Retorna o objeto Vessel (com ou sem os Holds, dependendo do parâmetro)
            return Ok(vessel);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar detalhes do Vessel {id}: {ex}");
            return StatusCode(500, "Erro interno ao buscar dados da embarcação.");
        }
    }

    // Método para receber os dados do celular e sincronizar
    [HttpPost("sync")] // Rota: POST /api/vessels/sync
    public async Task<IActionResult> SyncVessels([FromBody] List<Vessel> vesselsFromApp)
    {
        if (vesselsFromApp == null || !vesselsFromApp.Any())
        {
            return BadRequest("Nenhuma embarcação para sincronizar.");
        }

        // 1. Pega os GlobalIds de todas as embarcações que o app enviou
        var receivedGlobalIds = vesselsFromApp.Select(v => v.GlobalId).ToList();

        // 2. Verifica no banco quais desses GlobalIds já existem
        var existingGlobalIds = await _context.Vessels
            .Where(v_db => receivedGlobalIds.Contains(v_db.GlobalId))
            .Select(v_db => v_db.GlobalId)
            .ToListAsync();

        // 3. Filtra a lista, pegando apenas as embarcações que são realmente novas
        var newVessels = vesselsFromApp
            .Where(v_app => !existingGlobalIds.Contains(v_app.GlobalId))
            .ToList();

        // 4. Se houver novas, salva-as no banco de dados
        if (newVessels.Any())
        {
            await _context.Vessels.AddRangeAsync(newVessels);
            await _context.SaveChangesAsync();
        }

        return Ok(new { Message = $"Sincronização concluída. {newVessels.Count} novas embarcações adicionadas." });
    }
}