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
        public string Status { get; set; } = "Активна";
        public Driver Driver { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();

        // Подія для сповіщення про зміну статусу (вимога ТЗ)
        public event Action<string> OnStatusChanged;

        // Реалізація інтерфейсу для сортування за ціною
        public int CompareTo(Trip other)
        {
            if (other == null) return 1;
            return Price.CompareTo(other.Price);
        }
    }
}