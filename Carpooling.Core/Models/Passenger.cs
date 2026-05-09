namespace Carpooling.Core.Models
{
    public class Passenger : User
    {
        // Характеристика з таблиці 2.7
        public List<Booking> Bookings { get; set; }
        public List<string> FavoriteRoutes { get; set; } = new List<string>();

        public Passenger()
        {
            throw new NotImplementedException();
            // Role = "Пасажир";
            // Bookings = new List<Booking>();
        }
    }
}