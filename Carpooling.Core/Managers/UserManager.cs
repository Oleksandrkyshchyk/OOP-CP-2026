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

            throw new NotImplementedException();
        }

        public User Login(string login, string password)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }
    }
}