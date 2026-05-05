using System;
using System.Text.RegularExpressions;

namespace Carpooling.Core.Validators
{
    public static class UserValidator
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
    }
}