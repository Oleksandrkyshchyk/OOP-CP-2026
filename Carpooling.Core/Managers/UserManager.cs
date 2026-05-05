using Carpooling.Core.Interfaces;
using Carpooling.Core.Models;
using Carpooling.Core.Validators;
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
            _users = _storage.LoadUsers().ToList();
        }

        public bool Register(User newUser)
        {
            // Використовуємо валідатор (>= 8 символів)
            if (!DataValidator.IsValidLogin(newUser.Login) ||
                !DataValidator.IsValidPassword(newUser.Password))
                return false;

            // Перевіряємо унікальність логіна
            if (_users.Any(u => u.Login == newUser.Login))
                return false;

            _users.Add(newUser);
            _storage.SaveUsers(_users);
            return true;
        }

        public User Login(string login, string password)
        {
            // Пошук користувача за парою логін/пароль
            return _users.FirstOrDefault(u => u.Login == login && u.Password == password);
        }

        public List<User> GetAllUsers() => _users;
    }
}