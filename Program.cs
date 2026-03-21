using FlightManagementWeb.Data;
using FlightManagementWeb.Models;
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
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 9;
}).AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// -----------------------------------------------------------

var app = builder.Build(); // Duvar burada örüldü. Artık servis eklenemez.

// --- 2. PIPELINE / MIDDLEWARE AYARLARI ---

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