using Hangfire;
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
    }
}
