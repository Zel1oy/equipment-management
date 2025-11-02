using Auth0.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using PstInventory.Core.repository;
using PstInventory.Core.service;
using PstInventory.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString, b => b.MigrationsAssembly("PstInventory.Infrastructure")));

builder.Services.AddScoped<IEquipmentRepository, EfEquipmentRepository>();

builder.Services.AddScoped<EquipmentService>();

builder.Services
    .AddAuth0WebAppAuthentication(options => {
        options.Domain = builder.Configuration["Auth0:Domain"];
        options.ClientId = builder.Configuration["Auth0:ClientId"];
        options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- START: THIS IS THE MISSING CODE ---
// This code block runs on startup and applies your database migrations.
// We are leaving the try-catch out so that if migration fails,
// the app will crash and show you the REAL error.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<AppDbContext>();

    logger.LogInformation("Attempting to apply database migrations...");
    
    // This command creates the database and all tables.
    context.Database.Migrate();
    
    logger.LogInformation("Database migrations applied successfully.");
}
// --- END: THIS IS THE MISSING CODE ---


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// This was in the wrong place. It must come AFTER UseRouting.
app.UseAuthentication(); // <-- ADD THIS LINE
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();