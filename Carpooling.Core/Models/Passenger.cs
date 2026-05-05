namespace Carpooling.Core.Models
{
    public class Passenger : User
    {
        // Характеристика з таблиці 2.7
        public List<Booking> Bookings { get; set; }

        public Passenger()
        {
            Role = "Пасажир";
            Bookings = new List<Booking>();
        }
    }
}