using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FastFoodWebApplication.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Identity;
using FastFoodWebApplication.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FastFoodWebApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FastFoodWebApplicationContext") ?? throw new InvalidOperationException("Connection string 'FastFoodWebApplicationContext' not found.")));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<FastFoodWebApplicationContext>();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 5;
    options.Password.RequiredUniqueChars = 1;
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.Name = "YourAppCookieName";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.LoginPath = "/Account/Login";
    // ReturnUrlParameter requires 
    //using Microsoft.AspNetCore.Authentication.Cookies;
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
