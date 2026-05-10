using System;

namespace Carpooling.Core.Models
{
    public class Booking
    {
        // Характеристики
        public Passenger Passenger { get; set; }
        public Trip Trip { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
        public int SeatsCount { get; set; }

        // Поведінка

        // Перевірити актуальність: повертає true, якщо статус поїздки – Активна
        public bool IsActive()
        {
            // Якщо посилання на поїздку порожнє, бронювання не може бути активним
            if (Trip == null) return false;
            return Trip.Status == "Активна";
        }

        // Отримати деталі: формує короткий опис для особистого кабінету
        public string GetDetails()
        {
            if (Trip == null) return "Дані про поїздку відсутні";

            // Маршрут, дата, кількість місць
            return $"Маршрут: {Trip.DepartureCity} — {Trip.ArrivalCity}, " +
                   $"Дата: {Trip.DepartureTime:dd.MM.yyyy HH:mm}, " +
                   $"Місць: {SeatsCount}";
        }
    }
}