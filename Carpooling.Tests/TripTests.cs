using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carpooling.Core.Models;
using System;
using System.Collections.Generic;

namespace Carpooling.Tests
{
    [TestClass]
    public class TripTests
    {
        [TestMethod]
        public void CalculateFreeSeats_ShouldReturnRemainingSeats()
        {
            // Arrange
            var trip = new Trip { TotalSeats = 5 };
            // Додаємо бронювання на 2 місця
            trip.Bookings.Add(new Booking { SeatsCount = 2 });

            // Act
            // Впаде з NotImplementedException
            int freeSeats = trip.CalculateFreeSeats();

            // Assert
            Assert.AreEqual(3, freeSeats);
        }

        [TestMethod]
        public void ChangeStatus_ValidNewStatus_UpdatesSuccessfully()
        {
            // Arrange
            var trip = new Trip { Status = "Активна" };

            // Act
            // Впаде з NotImplementedException
            bool result = trip.ChangeStatus("Завершена");

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("Завершена", trip.Status);
        }

        [TestMethod]
        public void CompareTo_LowerPriceTrip_ReturnsNegativeValue()
        {
            // Arrange
            var currentTrip = new Trip { Price = 250.00m };
            var higherPriceTrip = new Trip { Price = 600.00m };

            // Act
            // Впаде з NotImplementedException
            int result = currentTrip.CompareTo(higherPriceTrip);

            // Assert
            Assert.IsTrue(result < 0);
        }

        [TestMethod]
        public void ChangeStatus_SameStatus_ReturnsFalse()
        {
            // Arrange
            var trip = new Trip { Status = "Активна" };

            // Act
            // Впаде з NotImplementedException
            bool result = trip.ChangeStatus("Активна");

            // Assert
            Assert.IsFalse(result);
        }
    }
}