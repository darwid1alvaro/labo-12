namespace LABO12.Services;

    public class NotificationService
    {
        public void SendNotification(string user)
        {
            Console.WriteLine($"📢 Notificación enviada a {user} en {DateTime.Now}");
        }
    }
