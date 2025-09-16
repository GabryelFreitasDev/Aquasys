using Aquasys.Core.Entities;
using System.Net.Http.Json;

public class ApiService
{
    private readonly HttpClient _httpClient;

    private const string BaseUrl = "http://localhost:5270"; // Substituir pelo endereço da sua API

    public ApiService()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
    }

    public async Task<bool> SincronizarVesselsAsync(List<Vessel> vessels)
    {
        try
        {
            // Envia a lista de embarcações para o endpoint "api/vessels/sync"
            var response = await _httpClient.PostAsJsonAsync("api/vessels/sync", vessels);
            return response.IsSuccessStatusCode; // Retorna true se a API respondeu com sucesso (código 2xx)
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro na sincronização: {ex.Message}");
            return false;
        }
    }
}