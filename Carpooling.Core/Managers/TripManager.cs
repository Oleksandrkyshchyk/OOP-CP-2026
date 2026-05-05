using Carpooling.Core.Interfaces;
using Carpooling.Core.Models;
using Carpooling.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carpooling.Core.Managers
{
    public class TripManager
    {
        private readonly IDataStorage _storage;
        private List<Trip> _trips;

        public TripManager(IDataStorage storage)
        {
            _storage = storage;
            _trips = _storage.LoadTrips().ToList();
        }

        // Створення поїздки водієм
        public bool CreateTrip(Trip newTrip)
        {
            // Тут ми можемо додати перевірку через TripValidator у майбутньому
            if (newTrip.DepartureTime < DateTime.Now || newTrip.Price <= 0)
                return false;

            _trips.Add(newTrip);
            _storage.SaveTrips(_trips);
            return true;
        }

        // Пошук поїздок за містом прибуття (використання LINQ)
        public List<Trip> SearchTrips(string destination)
        {
            return _trips
                .Where(t => t.ArrivalCity.Contains(destination, StringComparison.OrdinalIgnoreCase)
                            && t.Status == "Активна")
                .ToList();
        }

        // Сортування поїздок за ціною (використання IComparable)
        public List<Trip> GetSortedTrips()
        {
            var sortedList = _trips.ToList();
            sortedList.Sort(); // Викличе CompareTo в моделі Trip
            return sortedList;
        }

        public List<Trip> GetAllTrips() => _trips;
    }
}