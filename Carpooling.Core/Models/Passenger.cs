namespace Carpooling.Core.Models
{
    public class Passenger : User
    {
        // Характеристика
        public List<Booking> Bookings { get; set; }
        public List<string> FavoriteRoutes { get; set; } = new List<string>();

        public Passenger()
        {
            Role = "Пасажир";
            Bookings = new List<Booking>();
            FavoriteRoutes = new List<string>();
        }
    }
}