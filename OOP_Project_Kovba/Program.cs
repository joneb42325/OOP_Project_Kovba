using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OOP_Project_Kovba.Data;
using OOP_Project_Kovba.Models;
using OOP_Project_Kovba.Interfaces;
using OOP_Project_Kovba.Data.Repositories;
using OOP_Project_Kovba;

var builder = WebApplication.CreateBuilder(args);

// Подключение к MySQL
var connectionString = builder.Configuration.GetConnectionString("MyGhodPoolDb");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Настраиваем Identity на Entity Framework
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(3);
    options.SlidingExpiration = true;
});
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ITripRepository, TripRepository>();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();

builder.Services.AddScoped<ITripService, TripService>();

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

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
