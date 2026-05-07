using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carpooling.Core.Validators;
using System;

namespace Carpooling.Tests
{
    [TestClass]
    public class TripValidatorTests
    {
        [TestMethod]
        public void IsValidPrice_PositiveValue_ReturnsTrue()
        {
            // Arrange
            decimal price = 250.00m;

            // Act
            bool result = TripValidator.IsValidPrice(price);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidPrice_ZeroOrNegative_ReturnsFalse()
        {
            // Arrange
            decimal zeroPrice = 0;
            decimal negativePrice = -10.50m;

            // Act
            bool resultZero = TripValidator.IsValidPrice(zeroPrice);
            bool resultNegative = TripValidator.IsValidPrice(negativePrice);

            // Assert
            Assert.IsFalse(resultZero);
            Assert.IsFalse(resultNegative);
        }

        [TestMethod]
        public void IsValidSeats_CorrectRange_ReturnsTrue()
        {
            // Arrange
            int seats = 4;

            // Act
            bool result = TripValidator.IsValidSeats(seats);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidSeats_OutOfRange_ReturnsFalse()
        {
            // Arrange
            int tooMany = 51;
            int tooFew = 0;

            // Act
            bool resultHigh = TripValidator.IsValidSeats(tooMany);
            bool resultLow = TripValidator.IsValidSeats(tooFew);

            // Assert
            Assert.IsFalse(resultHigh);
            Assert.IsFalse(resultLow);
        }
    }
}