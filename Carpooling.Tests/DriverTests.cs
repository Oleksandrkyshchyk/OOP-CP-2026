using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carpooling.Core.Models;
using System.Collections.Generic;

namespace Carpooling.Tests
{
    [TestClass]
    public class DriverTests
    {
        [TestMethod]
        public void Driver_Constructor_SetsCorrectDefaultRole()
        {
            // Arrange & Act
            // Впаде через NotImplementedException
            var driver = new Driver();

            // Assert
            Assert.AreEqual("Водій", driver.Role);
        }

        [TestMethod]
        public void Driver_IsUser_InheritanceCheck()
        {
            // Arrange & Act
            var driver = new Driver();

            // Assert
            // Перевірка принципу спадкування (Inheritance)
            Assert.IsInstanceOfType(driver, typeof(User));
        }

        [TestMethod]
        public void Driver_OwnTrips_InitializesEmptyList()
        {
            // Arrange & Act
            var driver = new Driver();

            // Assert
            Assert.IsNotNull(driver.OwnTrips);
            Assert.AreEqual(0, driver.OwnTrips.Count);
        }

        [TestMethod]
        public void Driver_CanSetCarInfo()
        {
            // Arrange
            var driver = new Driver();
            string model = "Toyota Camry";
            string plate = "AA1234BB";

            // Act
            driver.CarModel = model;
            driver.LicensePlate = plate;

            // Assert
            Assert.AreEqual(model, driver.CarModel);
            Assert.AreEqual(plate, driver.LicensePlate);
        }
    }
}