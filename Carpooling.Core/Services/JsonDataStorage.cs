using Newtonsoft.Json;
using Carpooling.Core.Interfaces;
using Carpooling.Core.Models;
using System.Collections.Generic;
using System.IO;

namespace Carpooling.Core.Services
{
    public class JsonDataStorage : IDataStorage
    {
        private readonly string _usersFile = "users.json";
        private readonly string _tripsFile = "trips.json";

        public void SaveUsers(IEnumerable<User> users)
        {
            // Formatting.Indented робить файл читабельним для людини
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(_usersFile, json);
        }

        public IEnumerable<User> LoadUsers()
        {
            if (!File.Exists(_usersFile)) return new List<User>();
            string json = File.ReadAllText(_usersFile);
            return JsonConvert.DeserializeObject<List<User>>(json);
        }

        public void SaveTrips(IEnumerable<Trip> trips)
        {
            string json = JsonConvert.SerializeObject(trips, Formatting.Indented);
            File.WriteAllText(_tripsFile, json);
        }

        public IEnumerable<Trip> LoadTrips()
        {
            if (!File.Exists(_tripsFile)) return new List<Trip>();
            string json = File.ReadAllText(_tripsFile);
            return JsonConvert.DeserializeObject<List<Trip>>(json);
        }
    }
}