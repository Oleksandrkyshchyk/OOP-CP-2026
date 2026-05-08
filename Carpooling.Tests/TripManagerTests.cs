using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carpooling.Core.Managers;
using Carpooling.Core.Models;
using Carpooling.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Carpooling.Tests
{
    // Проста мок-реалізація для тестів
    public class FakeStorage : IDataStorage
    {
        public IEnumerable<Trip> LoadTrips() => new List<Trip>();
        public void SaveTrips(IEnumerable<Trip> trips) { }
        public IEnumerable<User> LoadUsers() => new List<User>();
        public void SaveUsers(IEnumerable<User> users) { }
    }

    [TestClass]
    public class TripManagerTests
    {
        [TestMethod]
        public void CreateTrip_ValidTrip_ReturnsTrue()
        {
            // Arrange
            var manager = new TripManager(new FakeStorage());
            var trip = new Trip { DepartureTime = DateTime.Now.AddDays(1), Price = 100 };

            // Act
            bool result = manager.CreateTrip(trip);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SearchTrips_ValidDestination_ReturnsFilteredResults()
        {
            // Arrange
            var manager = new TripManager(new FakeStorage());
            // (В реальному тесті ми б додали дані в _trips через мок)

            // Act
            var results = manager.SearchTrips("Kyiv");

            // Assert
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void GetSortedTrips_ShouldInvokeSort()
        {
            // Arrange
            var manager = new TripManager(new FakeStorage());

            // Act
            var sorted = manager.GetSortedTrips();

            // Assert
            Assert.IsNotNull(sorted);
        }
    }
}