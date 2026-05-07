using System;
using System.Collections.Generic;

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

        // Подія для SOLID (Events)
        public event Action<string> OnStatusChanged;

        // Метод-заглушка для розрахунку місць
        public int CalculateFreeSeats()
        {
            throw new NotImplementedException();
        }

        // Метод-заглушка для зміни статусу
        public bool ChangeStatus(string newStatus)
        {
            throw new NotImplementedException();
        }

        // Заглушка IComparable для сортування
        public int CompareTo(Trip other)
        {
            throw new NotImplementedException();
        }
    }
}