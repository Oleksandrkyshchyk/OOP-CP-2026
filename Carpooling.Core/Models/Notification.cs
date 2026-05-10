using System;

namespace Carpooling.Core.Models
{
    public class Notification
    {
        public string Message { get; set; } // Текст повідомлення
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Дата створення
        public User Receiver { get; set; } // Отримувач

        public Notification(){ }

        // Поведінка (Таблиця 2.14)
        public string FormatMessage(string eventType, object additionalData)
        {
            // Логіка формування тексту на основі типу події
            switch (eventType?.ToLower())
            {
                case "trip_cancelled":
                    return "На жаль, вашу поїздку було скасовано водієм.";

                case "new_booking":
                    return $"У вас нове бронювання! Місць: {additionalData}";

                case "status_updated":
                    return $"Статус вашої поїздки змінено на: {additionalData}";

                default:
                    return "У вас нове системне повідомлення.";
            }
        }
    }
}