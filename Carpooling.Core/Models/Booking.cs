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
        public bool IsActive()
        {
            throw new NotImplementedException();
        }

        public string GetDetails()
        {
            // Формує короткий опис для особистого кабінету (маршрут, дата, місця)
            throw new NotImplementedException();
        }
    }
}