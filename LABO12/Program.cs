using Hangfire;
using Hangfire.MemoryStorage;
using LABO12.Services;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<NotificationService>();


// Hangfire
builder.Services.AddHangfire(config =>
    config.UseMemoryStorage());

// Servidor de Hangfire
builder.Services.AddHangfireServer();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware básico
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHangfireDashboard();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Dashboard de Hangfire (ver jobs)
app.UseHangfireDashboard("/hangfire");

// Rutas MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();