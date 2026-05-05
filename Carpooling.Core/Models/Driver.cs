namespace Carpooling.Core.Models
{
    public class Driver : User
    {
        // Характеристики з таблиці 2.5
        public string CarModel { get; set; }
        public string LicensePlate { get; set; }
        public List<Trip> OwnTrips { get; set; }

        public Driver()
        {
            Role = "Водій";
            OwnTrips = new List<Trip>(); // Ініціалізація колекції
        }
    }
}