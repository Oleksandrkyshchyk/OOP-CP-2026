using System;

namespace Carpooling.Core.Models
{
    public class Notification
    {
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public User Receiver { get; set; }
    }
}