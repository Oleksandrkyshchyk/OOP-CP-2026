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
            // Завантажуємо поїздки зі сховища
            _trips = _storage.LoadTrips()?.ToList() ?? new List<Trip>();
        }

        // Збереження поточних змін у сховище
        public bool SaveChanges()
        {
            try
            {
                _storage.SaveTrips(_trips);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Створення поїздки водієм
        public bool CreateTrip(Trip newTrip)
        {
            if (newTrip == null) return false;

            if (!string.IsNullOrEmpty(newTrip.DepartureCity) && !string.IsNullOrEmpty(newTrip.ArrivalCity))
            {
                if (!TripValidator.AreCitiesValid(newTrip.DepartureCity, newTrip.ArrivalCity))
                    return false;
            }

            if (newTrip.TotalSeats > 0 && !TripValidator.IsValidSeats(newTrip.TotalSeats))
                return false;

            if (newTrip.Price > 0 && !TripValidator.IsValidPrice(newTrip.Price))
                return false;

            _trips.Add(newTrip);
            return SaveChanges(); // Використовуємо новий метод
        }

        // Пошук поїздок за містом прибуття
        public List<Trip> SearchTrips(string destination)
        {
            if (string.IsNullOrWhiteSpace(destination))
                return _trips.Where(t => t.Status == "Активна").ToList();

            return _trips
                .Where(t => t.ArrivalCity.Contains(destination.Trim(), StringComparison.OrdinalIgnoreCase)
                            && t.Status == "Активна")
                .ToList();
        }

        // Сортування поїздок за ціною
        public List<Trip> GetSortedTrips()
        {
            var sortedList = _trips.ToList();
            sortedList.Sort();
            return sortedList;
        }

        public List<Trip> GetAllTrips() => _trips;

        public bool UpdateTrip(Trip updatedTrip)
        {
            var index = _trips.FindIndex(t => t.DepartureTime == updatedTrip.DepartureTime && t.DriverLogin == updatedTrip.DriverLogin);

            if (index != -1)
            {
                _trips[index] = updatedTrip;
                return SaveChanges(); // Використовуємо новий метод
            }
            return false;
        }
    }
}