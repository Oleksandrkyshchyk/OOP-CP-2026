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

        // Створення поїздки водієм
        public bool CreateTrip(Trip newTrip)
        {
            if (newTrip == null) return false;

            // Якщо міста або місця задані — валідуємо їх суворо
            // Якщо ні — пропускаємо базову перевірку
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
            _storage.SaveTrips(_trips);
            return true;
        }

        // Пошук поїздок за містом прибуття за допомогою LINQ
        public List<Trip> SearchTrips(string destination)
        {
            if (string.IsNullOrWhiteSpace(destination))
                return _trips.Where(t => t.Status == "Активна").ToList();

            // Фільтрація: місто прибуття та лише активні поїздки
            return _trips
                .Where(t => t.ArrivalCity.Contains(destination.Trim(), StringComparison.OrdinalIgnoreCase)
                            && t.Status == "Активна")
                .ToList();
        }

        // Сортування поїздок за ціною (через IComparable у моделі Trip)
        public List<Trip> GetSortedTrips()
        {
            var sortedList = _trips.ToList();
            sortedList.Sort();
            return sortedList;
        }

        public List<Trip> GetAllTrips() => _trips;

        public bool UpdateTrip(Trip updatedTrip)
        {
            // Знаходимо стару поїздку в списку за ID або часом/маршрутом і замінюємо її
            var trips = GetAllTrips();
            var index = trips.FindIndex(t => t.DepartureTime == updatedTrip.DepartureTime && t.DriverLogin == updatedTrip.DriverLogin);

            if (index != -1)
            {
                trips[index] = updatedTrip;
                _storage.SaveTrips(trips); // Записуємо весь оновлений список у JSON
                return true;
            }
            return false;
        }
    }
}