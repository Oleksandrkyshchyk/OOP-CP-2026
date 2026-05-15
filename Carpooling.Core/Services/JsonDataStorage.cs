using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Carpooling.Core.Interfaces;
using Carpooling.Core.Models;

namespace Carpooling.Core.Services
{
    public class JsonDataStorage : IDataStorage
    {
        private const string UsersFile = "users.json";
        private const string TripsFile = "trips.json";

        // Завантаження користувачів
        public IEnumerable<User> LoadUsers()
        {
            if (!File.Exists(UsersFile)) return new List<User>();
            try
            {
                string json = File.ReadAllText(UsersFile);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                };
                return JsonSerializer.Deserialize<List<User>>(json, options) ?? new List<User>();
            }
            catch { return new List<User>(); }
        }

        // Збереження користувачів
        public void SaveUsers(IEnumerable<User> users)
        {
            // Серіалізація в JSON
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(UsersFile, json);
        }

        // Для поїздок
        public IEnumerable<Trip> LoadTrips()
        {
            if (!File.Exists(TripsFile)) return new List<Trip>();

            try
            {
                string json = File.ReadAllText(TripsFile);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                };

                return JsonSerializer.Deserialize<List<Trip>>(json, options) ?? new List<Trip>();
            }
            catch
            {
                return new List<Trip>();
            }
        }

        public void SaveTrips(IEnumerable<Trip> trips)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
            };

            string json = JsonSerializer.Serialize(trips, options);
            File.WriteAllText(TripsFile, json);
        }
    }
}