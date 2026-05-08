using System;

namespace Carpooling.Core.Models
{
    public class Notification
    {
        public string Message { get; set; } // Текст повідомлення
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Дата створення
        public User Receiver { get; set; } // Отримувач

        public Notification()
        {
            // заглушка для конструктора
            throw new NotImplementedException();
        }

        // Поведінка
        public string FormatMessage(string eventType, object additionalData)
        {
            throw new NotImplementedException();
        }
    }
}