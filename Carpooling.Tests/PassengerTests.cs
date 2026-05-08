using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carpooling.Core.Models;
using System.Collections.Generic;

namespace Carpooling.Tests
{
    [TestClass]
    public class PassengerTests
    {
        [TestMethod]
        public void Passenger_Constructor_SetsCorrectDefaultRole()
        {
            // Arrange & Act
            // Очікуємо падіння через NotImplementedException у конструкторі
            var passenger = new Passenger();

            // Assert
            Assert.AreEqual("Пасажир", passenger.Role);
        }

        [TestMethod]
        public void Passenger_IsUser_InheritanceCheck()
        {
            // Arrange & Act
            var passenger = new Passenger();

            // Assert
            // Перевірка спадкування (Inheritance) — обов'язкова вимога
            Assert.IsInstanceOfType(passenger, typeof(User));
        }

        [TestMethod]
        public void Passenger_Bookings_InitializesEmptyList()
        {
            // Arrange & Act
            var passenger = new Passenger();

            // Assert
            Assert.IsNotNull(passenger.Bookings);
            Assert.AreEqual(0, passenger.Bookings.Count);
        }
    }
}