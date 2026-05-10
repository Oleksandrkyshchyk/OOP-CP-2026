namespace Carpooling.Core.Models
{
    public class Driver : User
    {
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