using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carpooling.Core.Validators;
using System;

namespace Carpooling.Tests
{
    [TestClass]
    public class UserValidatorTests
    {
        [TestMethod]
        public void IsValidLogin_NormalLogin_ReturnsTrue()
        {
            // Arrange
            string login = "Alexander";

            // Act
            bool result = UserValidator.IsValidLogin(login);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidLogin_WithSpaces_ReturnsFalse()
        {
            // Arrange
            string login = "User Name";

            // Act
            bool result = UserValidator.IsValidLogin(login);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidPassword_LongEnough_ReturnsTrue()
        {
            // Arrange
            string pass = "SecurePass123";

            // Act
            bool result = UserValidator.IsValidPassword(pass);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidPhone_CorrectFormat_ReturnsTrue()
        {
            // Arrange
            string phone = "+380991234567";

            // Act
            bool result = UserValidator.IsValidPhone(phone);

            // Assert
            Assert.IsTrue(result);
        }
    }
}