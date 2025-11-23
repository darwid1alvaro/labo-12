using Hangfire;
using Hangfire.MemoryStorage;
using LABO12.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar Kestrel para escuchar en el puerto correcto
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

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
    // Render maneja HTTPS, no usar HSTS en producción
}
else
{
    // Solo redirigir a HTTPS en desarrollo local
    app.UseHttpsRedirection();
}

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