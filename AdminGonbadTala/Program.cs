using Core.ChildInfoService;
using Core.InfoKhademService;
using Core.TimeShitChild;
using DataAccess.Data;
using DataAccess.Repositories.childRepo;
using DataAccess.Repositories.ChildRepo;
using DataAccess.Repositories.InfoKhademRepo;

using DataAccess.Repositories.TimeshitChildRepo;
using Microsoft.EntityFrameworkCore;
using System.Globalization;



var cultureInfo = new CultureInfo("fa-IR");
cultureInfo.DateTimeFormat.Calendar = new PersianCalendar();

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GonbadDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IChildRepositories, ChildRepositories>();
builder.Services.AddScoped<ChildInfoService>();

builder.Services.AddScoped<ITimeShitChildRepositories, TimeshitChildRepositories>();
builder.Services.AddScoped<TimeShitService>();

builder.Services.AddScoped<IInfoKademRepositories, InfoKademRepositories>();
builder.Services.AddScoped<InfoKhademService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(4); // خروج خودکار بعد از ۸ ساعت بیکاری
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
//builder.Services.AddScoped<IChildRepository, ChildRepository>();
//builder.Services.AddScoped<IVisitRepository, VisitRepository>();
//builder.Services.AddScoped<GonbadTalaService>();

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
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "root",
    pattern: "",
    defaults: new { controller = "Account", action = "Login" });

// ۲. مسیر عمومی برای سایر کنترلرها (که اکشن پیش‌فرض آنها Index است)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Kids}/{action=Index}/{id?}");

app.Run();
