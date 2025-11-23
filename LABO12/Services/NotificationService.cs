namespace LABO12.Services;

    public class NotificationService
    {
        private static int _intentosFalloTemporal = 0;
        private static int _intentosFalloIntermitente = 0;

        public void SendNotification(string user)
        {
            Console.WriteLine($"📢 Notificación enviada a {user} en {DateTime.Now}");
        }

        // Método que SIEMPRE falla - para demostrar los reintentos automáticos
        public void SendNotificationWithError(string user)
        {
            Console.WriteLine($"❌ ERROR: Intentando enviar notificación a {user} en {DateTime.Now}");
            throw new Exception($"Error al enviar notificación a {user}. Hangfire reintentará automáticamente.");
        }

        // Método que falla las primeras 2 veces y luego funciona - para demostrar recuperación
        public void SendNotificationWithTemporaryError(string user)
        {
            _intentosFalloTemporal++;
            Console.WriteLine($"⚠️ Intento #{_intentosFalloTemporal} de enviar notificación a {user}");

            if (_intentosFalloTemporal < 3)
            {
                throw new Exception($"Error temporal #{_intentosFalloTemporal}. Hangfire reintentará...");
            }

            Console.WriteLine($"✅ Notificación enviada exitosamente a {user} después de {_intentosFalloTemporal} intentos");
            _intentosFalloTemporal = 0; // Resetear contador
        }

        // Método que falla intermitentemente (50% de probabilidad)
        public void SendNotificationWithIntermittentError(string user)
        {
            _intentosFalloIntermitente++;
            var random = new Random();

            if (random.Next(0, 2) == 0)
            {
                Console.WriteLine($"❌ Error intermitente en intento #{_intentosFalloIntermitente} para {user}");
                throw new Exception($"Error intermitente en el servicio externo. Reintentando...");
            }

            Console.WriteLine($"✅ Notificación enviada exitosamente a {user} en intento #{_intentosFalloIntermitente}");
            _intentosFalloIntermitente = 0;
        }

        // Método que simula timeout/error de red
        public void SendNotificationWithTimeout(string user)
        {
            Console.WriteLine($"⏳ Simulando timeout para {user}...");
            throw new TimeoutException($"Timeout al conectar con el servicio de notificaciones para {user}");
        }

        // Método que valida datos y falla si son inválidos
        public void SendNotificationWithValidation(string user)
        {
            if (string.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentException("El usuario no puede estar vacío");
            }

            if (user.Length < 3)
            {
                throw new ArgumentException("El nombre de usuario debe tener al menos 3 caracteres");
            }

            Console.WriteLine($"✅ Notificación validada y enviada a {user}");
        }
    }
