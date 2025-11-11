using Auth0.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using PstInventory.Core.repository;
using PstInventory.Core.service;
using PstInventory.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

string? provider = builder.Configuration["DatabaseProvider"];
string migrationsAssembly = "PstInventory.Infrastructure";

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
            // In-Memory doesn't need migrations or a connection string
            options.UseInMemoryDatabase("InMemoryDb");
            break;

        case "Sqlite":
        default: // Use SQLite as the default
            options.UseSqlite(
                builder.Configuration.GetConnectionString("Sqlite"),
                b => b.MigrationsAssembly(migrationsAssembly)
            );
            break;
    }
});

// Register your services (this is unchanged)
builder.Services.AddScoped<IEquipmentRepository, EfEquipmentRepository>();
builder.Services.AddScoped<EquipmentService>();

// Configure Auth0 (this is unchanged)
builder.Services
    .AddAuth0WebAppAuthentication(options => {
        options.Domain = builder.Configuration["Auth0:Domain"];
        options.ClientId = builder.Configuration["Auth0:ClientId"];
        options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- Automatic Migration Block ---
// This runs migrations on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<AppDbContext>();

    // This checks if the provider is In-Memory before trying to migrate
    if (!context.Database.IsInMemory())
    {
        logger.LogInformation("Attempting to apply database migrations...");
        
        // This command creates the database and all tables.
        context.Database.Migrate();
        
        logger.LogInformation("Database migrations applied successfully.");
    }
    else
    {
        logger.LogInformation("Using In-Memory database. Ensuring database is created...");
        
        // This command creates the schema AND runs the seed data
        context.Database.EnsureCreated();
        
        logger.LogInformation("In-Memory database created and seeded.");
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
