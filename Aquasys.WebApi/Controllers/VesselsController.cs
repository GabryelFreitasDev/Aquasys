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