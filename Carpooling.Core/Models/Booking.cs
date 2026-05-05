using System;

namespace Carpooling.Core.Models
{
    public class Booking
    {
        public Passenger Passenger { get; set; }
        public Trip Trip { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
        public int SeatsCount { get; set; }

        public bool IsActive() => throw new NotImplementedException();
    }
}