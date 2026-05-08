using System;
using System.Collections.Generic;
using Carpooling.Core.Interfaces;
using Carpooling.Core.Models;

namespace Carpooling.Core.Services
{
    public class JsonDataStorage : IDataStorage
    {
        public IEnumerable<Trip> LoadTrips() => throw new NotImplementedException();
        public void SaveTrips(IEnumerable<Trip> trips) => throw new NotImplementedException();
        public IEnumerable<User> LoadUsers() => throw new NotImplementedException();
        public void SaveUsers(IEnumerable<User> users) => throw new NotImplementedException();
    }
}