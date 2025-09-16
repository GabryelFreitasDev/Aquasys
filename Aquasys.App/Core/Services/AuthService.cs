using Aquasys.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.App.Core.Services
{
    public interface IAuthService
    {
        Task<User?> LoginAsync(string username, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            try
            {
                // O objeto que a API espera no endpoint /api/auth/login
                var loginRequest = new { Username = username, Password = password };

                var response = await _httpClient.PostAsJsonAsync("/api/Auth/login", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    // Se o login for bem-sucedido, a API retorna o objeto do usuário
                    var user = await response.Content.ReadFromJsonAsync<User>();
                    return user;
                }

                // Se a API retornar um erro (401 Unauthorized, etc.), retornamos nulo
                return null;
            }
            catch (HttpRequestException ex)
            {
                // Captura erros de rede (API fora do ar, sem conexão, etc.)
                System.Diagnostics.Debug.WriteLine($"Erro de rede na autenticação: {ex.Message}");
                return null;
            }
        }
    }
}
