using System;

namespace Carpooling.Core.Validators
{
    public static class TripValidator
    {
        // Перевірка міст
        public static bool AreCitiesValid(string departure, string arrival)
        {
            throw new NotImplementedException();
        }

        // Перевірка дати
        public static bool IsDateValid(DateTime departureTime)
        {
            throw new NotImplementedException();
        }

        // Перевірка ціни
        public static bool IsValidPrice(decimal price)
        {
            throw new NotImplementedException();
        }

        // Перевірка місць
        public static bool IsValidSeats(int seats)
        {
            throw new NotImplementedException();
        }
    }
}