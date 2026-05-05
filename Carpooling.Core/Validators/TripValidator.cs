using System;
using System.Text.RegularExpressions;

namespace Carpooling.Core.Validators
{
    public static class TripValidator
    {
        // Перевірка ціни (не може бути 0 або менше)
        public static bool IsValidPrice(decimal price)
        {
            return price > 0;
        }

        // Перевірка кількості місць (від 1 до 50)
        public static bool IsValidSeats(int seats)
        {
            return seats >= 1 && seats <= 50;
        }
    }
}