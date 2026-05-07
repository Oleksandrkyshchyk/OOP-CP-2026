using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carpooling.Core.Models;

namespace Carpooling.Tests
{
    [TestClass]
    public class BookingTests
    {
        [TestMethod]
        public void IsActive_WhenTripIsActive_ReturnsTrue()
        {
            // Arrange
            var trip = new Trip { Status = "Активна" };
            var booking = new Booking { Trip = trip };

            // Act
            // Впаде з NotImplementedException
            bool result = booking.IsActive();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsActive_WhenTripIsCancelled_ReturnsFalse()
        {
            // Arrange
            var trip = new Trip { Status = "Скасована" };
            var booking = new Booking { Trip = trip };

            // Act
            bool result = booking.IsActive();

            // Assert
            Assert.IsFalse(result);
        }
    }
}