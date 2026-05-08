using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carpooling.Core.Models;

namespace Carpooling.Tests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Admin_Constructor_SetsDefaultRoleAndAccessLevel()
        {
            // Arrange & Act
            var admin = new Admin();

            // Assert
            // Перевіряємо, чи конструктор правильно встановлює початкові дані
            Assert.AreEqual("Адміністратор", admin.Role);
            Assert.AreEqual(1, admin.AccessLevel);
        }

        [TestMethod]
        public void Admin_IsUser_InheritanceCheck()
        {
            // Arrange
            var admin = new Admin();

            // Act & Assert
            // Перевірка спадкування: об'єкт Admin має бути сумісним з типом User
            Assert.IsInstanceOfType(admin, typeof(User));
        }

        [TestMethod]
        public void Admin_CanSetAccessLevel()
        {
            // Arrange
            var admin = new Admin();
            int newLevel = 5;

            // Act
            admin.AccessLevel = newLevel;

            // Assert
            Assert.AreEqual(newLevel, admin.AccessLevel);
        }
    }
}