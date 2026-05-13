using System;
using System.Collections.Generic;
using System.Linq;

namespace Carpooling.Core.Models
{
    public class Trip : IComparable<Trip>
    {

        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public DateTime DepartureTime { get; set; }
        public decimal Price { get; set; }
        public int TotalSeats { get; set; }
        public string Status { get; set; } // Активна, Завершена, Скасована
        public List<Booking> Bookings { get; set; } = new List<Booking>();

        public event Action<string> OnStatusChanged;

        // Реалізація розрахунку вільних місць
        public int CalculateFreeSeats()
        {
            // Віднімаємо кількість заброньованих місць від загальної кількості
            int reservedSeats = Bookings?.Sum(b => b.SeatsCount) ?? 0;
            return TotalSeats - reservedSeats;
        }

        // Реалізація зміни статусу
        public bool ChangeStatus(string newStatus)
        {
            if (string.IsNullOrWhiteSpace(newStatus)) return false;

            // Якщо новий статус збігається з поточним — повертаємо false
            if (Status == newStatus) return false;

            Status = newStatus;

            // Викликаємо подію лише при реальній зміні
            OnStatusChanged?.Invoke(newStatus);

            return true;
        }

        // Реалізація IComparable для сортування
        public int CompareTo(Trip other)
        {
            if (other == null) return 1;

            // Сортуємо за ціною
            return Price.CompareTo(other.Price);
        }
    }
}