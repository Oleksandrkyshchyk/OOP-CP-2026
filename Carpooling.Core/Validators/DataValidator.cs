using System;
using System.Text.RegularExpressions;

namespace Carpooling.Core.Validators
{
    public static class DataValidator
    {
        // Перевірка логіна (не менше 3 символів, без пробілів)
        public static bool IsValidLogin(string login)
        {
            return !string.IsNullOrWhiteSpace(login) && login.Length >= 3 && !login.Contains(" ");
        }

        // Перевірка пароля (мінімум 8 символів)
        public static bool IsValidPassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length >= 8;
        }

        // Перевірка номера телефону (формат +380...)
        public static bool IsValidPhone(string phone)
        {
            string pattern = @"^\+380\d{9}$";
            return Regex.IsMatch(phone, pattern);
        }

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