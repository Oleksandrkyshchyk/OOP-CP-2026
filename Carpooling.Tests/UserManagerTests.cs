using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carpooling.Core.Managers;
using Carpooling.Core.Models;
using System;

namespace Carpooling.Tests
{
    [TestClass]
    public class UserManagerTests
    {
        [TestMethod]
        public void Register_ValidUser_ReturnsTrue()
        {
            // Arrange
            var manager = new UserManager(new FakeStorage());
            var user = new Passenger { Login = "test_user", Password = "password123" };

            // Act
            bool result = manager.Register(user);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Register_DuplicateLogin_ReturnsFalse()
        {
            // Arrange
            var manager = new UserManager(new FakeStorage());
            var user1 = new Passenger { Login = "user1", Password = "password123" };
            // Припускаємо, що один користувач вже є (це ми реалізуємо в 3.4)

            // Act
            bool result = manager.Register(user1);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Login_CorrectCredentials_ReturnsUser()
        {
            // Arrange
            var manager = new UserManager(new FakeStorage());

            // Act
            var result = manager.Login("admin", "admin123");

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetAllUsers_ReturnsList()
        {
            // Arrange
            var manager = new UserManager(new FakeStorage());

            // Act
            var users = manager.GetAllUsers();

            // Assert
            Assert.IsNotNull(users);
        }
    }
}