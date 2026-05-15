using System;
using System.Text.RegularExpressions;

namespace Carpooling.Core.Validators
{
    public static class UserValidator
    {
        // 1. Перевірка логіна та імені
        public static bool IsValidLogin(string login)
        {
            // 1. Перевірка на null або порожній рядок
            if (string.IsNullOrWhiteSpace(login))
                return false;

            // 2. Перевірка довжини (мінімум 3 символи)
            if (login.Length < 3)
                return false;

            // 3. Перевірка на відсутність пробілів
            if (login.Contains(" "))
                return false;

            return true;
        }

        // 2. Перевірка пароля
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            // Перевірка на велику літеру та цифру
            bool hasUpper = Regex.IsMatch(password, @"[A-Z]");
            bool hasDigit = Regex.IsMatch(password, @"\d");

            return hasUpper && hasDigit;
        }

        // 3. Валідація формату телефону
        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;

            // Формат +380XXXXXXXXX
            string pattern = @"^\+380\d{9}$";
            return Regex.IsMatch(phone, pattern);
        }

        // 4. Валідація Прізвища та Ім'я (Додано)
        public static bool IsValidFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return false;

            // Мінімум 3 символи, дозволяємо літери, пробіли та апостроф
            // Регулярний вираз перевіряє, щоб були лише літери різних регістрів
            string pattern = @"^[a-zA-Zа-яА-ЯіІїЇєЄґҐ' ]{3,50}$";
            return Regex.IsMatch(fullName, pattern);
        }
    }
}