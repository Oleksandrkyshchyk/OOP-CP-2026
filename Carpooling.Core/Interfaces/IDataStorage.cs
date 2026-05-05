using Carpooling.Core.Models;
using System.Collections.Generic;

namespace Carpooling.Core.Interfaces
{
    /// <summary>
    /// Інтерфейс для керування збереженням та завантаженням даних системи
    /// </summary>
    public interface IDataStorage
    {
        // Робота з користувачами
        void SaveUsers(IEnumerable<User> users);
        IEnumerable<User> LoadUsers();

        // Робота з поїздками
        void SaveTrips(IEnumerable<Trip> trips);
        IEnumerable<Trip> LoadTrips();
    }
}