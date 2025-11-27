using Auth0.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using PstInventory.Core.repository;
using PstInventory.Core.service;
using PstInventory.Infrastructure.Data;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

string? provider = builder.Configuration["DatabaseProvider"];
string migrationsAssembly = "PstInventory.Infrastructure";

// ---------- БД ----------
builder.Services.AddDbContext<AppDbContext>(options =>
{
    switch (provider)
    {
        case "SqlServer":
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("SqlServer"),
                b => b.MigrationsAssembly(migrationsAssembly)
            );
            break;

        case "Postgres":
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("Postgres"),
                b => b.MigrationsAssembly(migrationsAssembly)
            );
            break;

        case "InMemory":
            options.UseInMemoryDatabase("InMemoryDb");
            break;

        case "Sqlite":
        default:
            options.UseSqlite(
                builder.Configuration.GetConnectionString("Sqlite"),
                b => b.MigrationsAssembly(migrationsAssembly)
            );
            break;
    }
});

// ---------- Репозиторії + сервіси ----------
builder.Services.AddScoped<IEquipmentRepository, EfEquipmentRepository>();
builder.Services.AddScoped<EquipmentService>();

// ---------- MVC + Swagger ----------
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------- OpenTelemetry ----------
builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService("PstInventory.WebApp"))
    .WithTracing(tracer =>
    {
        tracer
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSqlClientInstrumentation(o =>
            {
                // щоб у трейсах було видно SQL
                o.SetDbStatementForText = true;
            })
            .AddSource("PstInventory.WebApp")
            .AddZipkinExporter(o =>
            {
                // важливо: порт 9411, шлях /api/v2/spans
                o.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
            });
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddRuntimeInstrumentation()        // GC, heap і т.д.
            .AddAspNetCoreInstrumentation()
            .AddPrometheusExporter();
    });

var app = builder.Build();

// ---------- Pipeline ----------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Equipment API v1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// якщо auth вимкнена — ці два можна лишити закоментованими
// app.UseAuthentication();
// app.UseAuthorization();

// endpoint для Prometheus (на  /metrics)
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// --------- ActivitySource для своїх span-ів ---------
public static class Telemetry
{
    public static readonly ActivitySource ActivitySource = new("PstInventory.WebApp");
}
