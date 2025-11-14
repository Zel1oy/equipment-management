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

// репозиторій + сервіс
builder.Services.AddScoped<IEquipmentRepository, EfEquipmentRepository>();
builder.Services.AddScoped<EquipmentService>();

// Auth0
//builder.Services
//    .AddAuth0WebAppAuthentication(options =>
//    {
//        options.Domain = builder.Configuration["Auth0:Domain"];
//        options.ClientId = builder.Configuration["Auth0:ClientId"];
//        options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
//    });

// MVC
builder.Services.AddControllersWithViews();

// Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   // без OpenApiInfo


var app = builder.Build();

// міграції
/*using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<AppDbContext>();

    if (!context.Database.IsInMemory())
    {
        logger.LogInformation("Attempting to apply database migrations...");
        context.Database.Migrate();
        logger.LogInformation("Database migrations applied successfully.");
    }
    else
    {
        logger.LogInformation("Using In-Memory database. Ensuring database is created...");
        context.Database.EnsureCreated();
        logger.LogInformation("In-Memory database created and seeded.");
    }
}*/

// pipeline
if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Equipment API v1");
            
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Equipment API v2");
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

app.UseRouting();

// НІЯКОГО auth, усе відкрито — цього достатньо для ЛР-5
// app.UseAuthentication();
// app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
