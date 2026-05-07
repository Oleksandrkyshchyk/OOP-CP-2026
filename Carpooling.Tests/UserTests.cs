using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carpooling.Core.Models;
using System;

namespace Carpooling.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void ChangePassword_ValidNewPassword_UpdatesSuccessfully()
        {
            // Arrange
            var user = new Passenger { Password = "OldPassword123" };
            string newPass = "NewSecurePass888";

            // Act
            bool result = user.ChangePassword(newPass);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newPass, user.Password);
        }

        [TestMethod]
        public void ChangePassword_PasswordTooShort_ReturnsFalse()
        {
            // Arrange
            var user = new Passenger { Password = "OldPassword123" };
            string shortPass = "123"; // Менше 8 символів

            // Act
            bool result = user.ChangePassword(shortPass);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetRoleName_ReturnsCorrectRoleString()
        {
            // Arrange
            var user = new Passenger { Role = "Пасажир" };

            // Act
            string roleName = user.GetRoleName();

            // Assert
            Assert.AreEqual("Пасажир", roleName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ChangePassword_EmptyPassword_ThrowsArgumentException()
        {
            // Arrange
            var user = new Passenger { Password = "OldPassword123" };

            // Act
            // при порожньому паролі буде помилка
            user.ChangePassword("");
        }
    }
}