using Carpooling.Core.Interfaces;
using Carpooling.Core.Models;
using Carpooling.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carpooling.Core.Managers
{
    public class UserManager
    {
        private readonly IDataStorage _storage;
        private List<User> _users;

        public UserManager(IDataStorage storage)
        {
            _storage = storage;
            // Завантажуємо існуючих користувачів при старті
            _users = _storage.LoadUsers()?.ToList() ?? new List<User>();
        }

        public bool Register(User newUser)
        {
            if (newUser == null || string.IsNullOrWhiteSpace(newUser.Login)) return false;

            // Обов'язково перевіряємо унікальність без урахування регістру
            if (_users.Any(u => u.Login.Equals(newUser.Login.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            _users.Add(newUser);
            _storage.SaveUsers(_users);
            return true;
        }

        public User Login(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                return null;

            string trimmedLogin = login.Trim();

            // Шукаємо користувача: логін - IgnoreCase, пароль - Case-Sensitive
            return _users.FirstOrDefault(u => u.Login.Equals(trimmedLogin, StringComparison.OrdinalIgnoreCase) && u.Password == password);
        }

        // Отримання списку для Адміністратора
        public List<User> GetAllUsers()
        {
            return _users;
        }

        public void SaveChanges()
        {
            _storage.SaveUsers(_users);
        }
    }
}