using FlightManagementWeb.Data;
using FlightManagementWeb.Models;
using FlightManagementWeb.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// --- 1. SERVİS TANIMLAMALARI (builder.Build() öncesi olmalı) ---

builder.Services.AddControllersWithViews();

// Veritabanı bağlantısı
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Identity (Kimlik) sistemi
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        
        //options.Password.RequireDigit = true;
        //options.Password.RequireLowercase = true;
        //options.Password.RequireUppercase = true;
        //options.Password.RequireNonAlphanumeric = true;
        // options.Password.RequiredLength = 9;
    }).AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<PurchaseService>();

// -----------------------------------------------------------

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = { "Admin", "Manager", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    string email = "admin@admin.com";
    string password = "Admin123!";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new ApplicationUser { UserName = email, Email = email, FirstName = "Admin", LastName = "Admin" };
        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
            await userManager.AddToRoleAsync(user, "Admin");
    }
}



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// CRITICAL: Kimlik doğrulama, yetkilendirmeden ÖNCE gelmelidir.
app.UseAuthentication(); 


app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();