using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carpooling.Core.Services;
using Carpooling.Core.Models;
using System.Collections.Generic;
using System;

namespace Carpooling.Tests
{
    [TestClass]
    public class JsonDataStorageTests
    {
        [TestMethod]
        public void SaveUsers_ShouldExecuteSuccessfully()
        {
            // Arrange
            var storage = new JsonDataStorage();
            var users = new List<User> { new Passenger { Login = "test" } };

            // Act & Assert
            // Впаде через NotImplementedException
            storage.SaveUsers(users);
        }

        [TestMethod]
        public void LoadTrips_ShouldReturnEnumerable()
        {
            // Arrange
            var storage = new JsonDataStorage();

            // Act
            var result = storage.LoadTrips();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}