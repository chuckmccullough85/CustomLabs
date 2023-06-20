using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AcmeBank.Data;
using AcmeBank.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AcmeBankContextConnection") ?? throw new InvalidOperationException("Connection string 'AcmeBankContextConnection' not found.");

builder.Services.AddDbContext<AcmeBankContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<AcmeBankUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AcmeBankContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BankDbContext>();
builder.Services.AddScoped<IBankService, BankService>();

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

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
