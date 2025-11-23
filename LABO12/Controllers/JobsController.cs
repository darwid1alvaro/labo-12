using Hangfire;
using LABO12.Services;
using Microsoft.AspNetCore.Mvc;

namespace LABO12.Controllers
{
    public class JobsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FireAndForget()
        {
            BackgroundJob.Enqueue(() => Console.WriteLine($"🔥 Job Fire-and-Forget ejecutado en {DateTime.Now}"));
            ViewBag.Message = "✅ Job Fire-and-Forget encolado (ejecutado una sola vez)";
            return View("JobStatus");
        }

        public IActionResult Delayed()
        {
            BackgroundJob.Schedule(() => Console.WriteLine($"⏱️ Job programado ejecutado en {DateTime.Now}"), TimeSpan.FromSeconds(10));
            ViewBag.Message = "⏱️ Job programado para ejecutarse en 10 segundos";
            return View("JobStatus");
        }

        public IActionResult Recurrent()
        {
            RecurringJob.AddOrUpdate("job-diario",
                () => Console.WriteLine($"🔁 Job recurrente ejecutado en {DateTime.Now}"),
                Cron.Minutely);
            ViewBag.Message = "🔁 Job recurrente configurado (cada minuto)";
            return View("JobStatus");
        }

        // PASO 8: SIMULACIÓN DE ERRORES Y REINTENTOS

        // Job que SIEMPRE falla - demuestra los reintentos automáticos de Hangfire
        public IActionResult JobWithError()
        {
            BackgroundJob.Enqueue<NotificationService>(service =>
                service.SendNotificationWithError("usuario_con_error"));
            ViewBag.Message = "❌ Job encolado que SIEMPRE falla - Revisa el Dashboard para ver los reintentos automáticos";
            return View("JobStatus");
        }

        // Job con error temporal - falla 2 veces y luego funciona
        public IActionResult JobWithTemporaryError()
        {
            BackgroundJob.Enqueue<NotificationService>(service =>
                service.SendNotificationWithTemporaryError("usuario_temporal"));
            ViewBag.Message = "⚠️ Job con error temporal encolado - Fallará 2 veces y luego funcionará";
            return View("JobStatus");
        }

        // Job con error intermitente - 50% probabilidad de fallo
        public IActionResult JobWithIntermittentError()
        {
            BackgroundJob.Enqueue<NotificationService>(service =>
                service.SendNotificationWithIntermittentError("usuario_intermitente"));
            ViewBag.Message = "🎲 Job con error intermitente encolado - 50% de probabilidad de fallo";
            return View("JobStatus");
        }

        // Job con timeout
        public IActionResult JobWithTimeout()
        {
            BackgroundJob.Enqueue<NotificationService>(service =>
                service.SendNotificationWithTimeout("usuario_timeout"));
            ViewBag.Message = "⏳ Job con timeout encolado - Simulará un error de conexión";
            return View("JobStatus");
        }

        // Job con validación de datos
        public IActionResult JobWithValidation()
        {
            // Este fallará porque el usuario está vacío
            BackgroundJob.Enqueue<NotificationService>(service =>
                service.SendNotificationWithValidation(""));
            ViewBag.Message = "🔍 Job con validación encolado (usuario vacío) - Fallará por validación";
            return View("JobStatus");
        }

        // Job con validación exitosa
        public IActionResult JobWithValidationSuccess()
        {
            BackgroundJob.Enqueue<NotificationService>(service =>
                service.SendNotificationWithValidation("UsuarioValido"));
            ViewBag.Message = "✅ Job con validación exitosa encolado";
            return View("JobStatus");
        }

        // Job delayed con error
        public IActionResult DelayedJobWithError()
        {
            BackgroundJob.Schedule<NotificationService>(
                service => service.SendNotificationWithError("usuario_delayed_error"),
                TimeSpan.FromSeconds(5));
            ViewBag.Message = "⏱️❌ Job programado con error para 5 segundos - Verás los reintentos en el Dashboard";
            return View("JobStatus");
        }

        // Job recurrente con error temporal
        public IActionResult RecurringJobWithError()
        {
            RecurringJob.AddOrUpdate<NotificationService>(
                "job-recurrente-con-error",
                service => service.SendNotificationWithTemporaryError("usuario_recurrente"),
                Cron.Minutely);
            ViewBag.Message = "🔁⚠️ Job recurrente con error configurado (cada minuto) - Verás el patrón de fallos/éxitos";
            return View("JobStatus");
        }
    }
}
