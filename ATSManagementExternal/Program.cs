using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using ATSManagementExternal.IModels;
using ATSManagementExternal.Models;
using ATSManagementExternal.Services;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AtsdbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ATSDB"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 200,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorNumbersToAdd: null);
        });

});
// Add services to the container.
builder.Services.AddServerSideBlazor(); // Add support for Blazor
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(60000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddRazorPages().AddNToastNotifyNoty(new NotyOptions
{
    ProgressBar = true,
    Timeout = 5000,

});
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 10;
    config.HasRippleEffect = true;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
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
app.UseSession();
app.UseRouting();

app.UseAuthorization();
app.UseNToastNotify();
app.UseNotyf();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
