using Aquasys.WebApi.Data;
using Aquasys.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Licensing;

// Adicione estes using para os relatórios:
using Aquasys.Reports.Services;
using Aquasys.Reports.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddSingleton<SyncTypeRegistry>();
SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF5cX2BCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWH5cdXVQRGBZUUNwWUpWYEg=");

// === REGISTRO DOS SERVIÇOS DO RELATÓRIO ===
builder.Services.AddScoped<ReportGeneratorService>();

// Registrar todos os templates de relatório implementando IReportTemplate
builder.Services.Scan(scan => scan
    .FromAssemblyOf<ReportGeneratorService>()
    .AddClasses(classes => classes.AssignableTo<IReportTemplate>())
    .AsImplementedInterfaces()
    .WithScopedLifetime());
// ==========================================

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();