using Microsoft.VisualStudio.TestTools.UnitTesting;
using Carpooling.Core.Models;
using System;

namespace Carpooling.Tests
{
    [TestClass]
    public class NotificationTests
    {
        [TestMethod]
        public void Notification_Constructor_InitializesDateTime()
        {
            // Arrange & Act
            var notification = new Notification();

            // Assert
            // Дата має встановлюватися автоматично
            Assert.IsTrue(notification.CreatedAt <= DateTime.Now);
        }

        [TestMethod]
        public void FormatMessage_ShouldReturnFormattedString()
        {
            // Arrange
            var notification = new Notification();
            string eventType = "TripCancelled";

            // Act
            string result = notification.FormatMessage(eventType, null);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Notification_CanAssignReceiver()
        {
            // Arrange
            var notification = new Notification();
            var passenger = new Passenger { Login = "passenger_1" };

            // Act
            notification.Receiver = passenger;

            // Assert
            // Отримувач має бути об'єктом класу User
            Assert.AreEqual("passenger_1", notification.Receiver.Login);
        }
    }
}