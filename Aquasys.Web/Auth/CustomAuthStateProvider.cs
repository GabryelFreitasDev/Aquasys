using Aquasys.Core.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using System.Text.Json; // Importante para serialização

namespace Aquasys.Web.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        // Usaremos a session storage (ou local storage) para persistir o login entre abas/reloads
        private readonly ProtectedSessionStorage _sessionStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        private ClaimsPrincipal _cachedPrincipal = new ClaimsPrincipal(new ClaimsIdentity()); // Começa anónimo
        private bool _hasCheckedStorage = false;

        public CustomAuthStateProvider(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Se já lemos o storage antes, apenas retornamos o principal em cache
            if (_hasCheckedStorage)
            {
                return new AuthenticationState(_cachedPrincipal);
            }

            // --- LÓGICA CRÍTICA ---
            // Só tentamos ler do sessionStorage se NÃO estivermos na pré-renderização.
            // A forma mais segura de verificar isso é tentar ler. Se der erro, estamos em SSR.
            User? user = null;
            try
            {
                var userSessionResult = await _sessionStorage.GetAsync<User>("CurrentUser");
                user = userSessionResult.Success ? userSessionResult.Value : null;
                _hasCheckedStorage = true; // Marcamos que tentamos (com sucesso ou não) ler o storage
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("JavaScript interop calls"))
            {
                // Estamos em pré-renderização estática. Retornamos anónimo e NÃO marcamos _hasCheckedStorage.
                // O Blazor chamará este método novamente após a conexão interativa.
                Console.WriteLine("GetAuthStateAsync: Pré-renderização detectada. Retornando anónimo."); // Log para depuração
                return new AuthenticationState(_cachedPrincipal);
            }
            catch (Exception ex) // Outros erros
            {
                Console.WriteLine($"Erro ao ler SessionStorage: {ex.Message}");
                _hasCheckedStorage = true; // Marcamos como verificado mesmo em erro
                return new AuthenticationState(_cachedPrincipal); // Erro = anónimo
            }


            if (user == null)
            {
                _cachedPrincipal = new ClaimsPrincipal(new ClaimsIdentity()); // Anónimo
            }
            else
            {
                // Cria Claims e Principal se o usuário foi encontrado
                var claims = new[] {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(ClaimTypes.NameIdentifier, user.GlobalId.ToString())
                };
                var identity = new ClaimsIdentity(claims, "apiauth");
                _cachedPrincipal = new ClaimsPrincipal(identity); // Guarda o principal logado
            }

            return new AuthenticationState(_cachedPrincipal);
        }

        // Método chamado pelo Login.razor após sucesso na API
        public async Task MarkUserAsAuthenticated(User user)
        {
            await _sessionStorage.SetAsync("CurrentUser", user); // Salva na session storage
            var claims = new[] {  new Claim(ClaimTypes.Name, user.UserName),
                                  new Claim(ClaimTypes.Email, user.Email ?? ""),
                                  new Claim(ClaimTypes.NameIdentifier, user.GlobalId.ToString()) };
            var identity = new ClaimsIdentity(claims, "apiauth");
            var principal = new ClaimsPrincipal(identity);

            // Notifica o Blazor que o estado de autenticação mudou!
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        // Método para Logout
        public async Task MarkUserAsLoggedOut()
        {
            await _sessionStorage.DeleteAsync("CurrentUser"); // Remove da session storage
                                                              // Notifica o Blazor que o usuário deslogou
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}