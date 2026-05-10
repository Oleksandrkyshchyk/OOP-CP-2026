using System;

namespace Carpooling.Core.Validators
{
    public static class TripValidator
    {
        // 1. Перевірка міст: не мають збігатися
        public static bool AreCitiesValid(string departure, string arrival)
        {
            if (string.IsNullOrWhiteSpace(departure) || string.IsNullOrWhiteSpace(arrival))
                return false;

            return !departure.Trim().Equals(arrival.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        // 2. Перевірка дати: не раніше поточної
        public static bool IsDateValid(DateTime departureTime)
        {
            // Використовуємо невеликий запас, 
            // щоб тест не впав через мілісекундну різницю під час виконання
            return departureTime >= DateTime.Now.AddMinutes(-1);
        }

        // 3. Перевірка ціни: має бути > 0
        public static bool IsValidPrice(decimal price)
        {
            return price > 0;
        }

        // 4. Перевірка місць: від 1 до 50
        public static bool IsValidSeats(int seats)
        {
            // Обмеження: додатне число до 50
            return seats > 0 && seats <= 50;
        }
    }
}