using Aquasys.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDevExpressBlazor();

// Adiciona um HttpClient configurado para se comunicar com a sua API
builder.Services.AddHttpClient("WebApi", client =>
{
    // Use a URL base da sua API.
    // Se você roda a API e o Web App ao mesmo tempo, esta URL (localhost) funciona.
    // Certifique-se de que a porta (ex: 7182) está correta.
    client.BaseAddress = new Uri("https://localhost:7182");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
