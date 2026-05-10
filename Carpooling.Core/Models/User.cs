using System;
using Carpooling.Core.Validators;

namespace Carpooling.Core.Models
{
    // Абстрактний клас
    public abstract class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }

        // Метод для отримання назви ролі
        public virtual string GetRoleName()
        {
            // Повертаємо встановлену роль або "Користувач" за замовчуванням
            return !string.IsNullOrEmpty(Role) ? Role : "Користувач";
        }

        // Метод для зміни пароля
        public virtual bool ChangePassword(string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentException("Пароль не може бути порожнім");
            }

            if (UserValidator.IsValidPassword(newPassword))
            {
                Password = newPassword;
                return true;
            }

            return false;
        }
    }
}