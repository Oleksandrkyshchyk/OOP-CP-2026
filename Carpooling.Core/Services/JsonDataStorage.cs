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
                    PropertyNameCaseInsensitive = true, // Ігнорувати регістр назв (Login vs login)
                    WriteIndented = true
                };

                return JsonSerializer.Deserialize<List<User>>(json, options) ?? new List<User>();
            }
            catch (Exception ex)
            {
                return new List<User>();
            }
        }

        // Збереження користувачів
        public void SaveUsers(IEnumerable<User> users)
        {
            // Серіалізація в JSON
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(UsersFile, json);
        }

        // Аналогічна логіка для поїздок
        public IEnumerable<Trip> LoadTrips()
        {
            if (!File.Exists(TripsFile)) return new List<Trip>();

            try
            {
                string json = File.ReadAllText(TripsFile);
                return JsonSerializer.Deserialize<List<Trip>>(json) ?? new List<Trip>();
            }
            catch
            {
                return new List<Trip>();
            }
        }

        public void SaveTrips(IEnumerable<Trip> trips)
        {
            string json = JsonSerializer.Serialize(trips, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(TripsFile, json);
        }
    }
}