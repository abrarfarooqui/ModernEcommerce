using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Services.Implementation;
using Ecommerce.Application.Services.Interface;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repository;
using ECommerce.Application.Common.Utility;
using ECommerce.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Syncfusion.Licensing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IVillaService, VillaService>();
builder.Services.AddScoped<IVillaNumberService, VillaNumberService>();
builder.Services.AddScoped<IAmenityService, AmenityService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<UploadFile>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath= "/Account/AccessDenied";
    options.LoginPath = "/Account/Login";
});

//Email sender configuration
var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
string smtpHost = emailSettings.smtpHost;
int smtpPort = emailSettings.smtpPort;
string smtpUser = emailSettings.smtpUser;
string smtpPass = emailSettings.smtpPass;
builder.Services.AddSingleton(emailSettings);

// Example: Reading individual values

var app = builder.Build();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

SyncfusionLicenseProvider.RegisterLicense(builder.Configuration.GetSection("Syncfusion:LicenseKey").Get<string>());

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
SeedDatabase();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
