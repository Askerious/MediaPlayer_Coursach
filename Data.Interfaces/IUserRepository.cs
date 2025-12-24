using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Data.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User GetById(int id);
        bool CheckPassword(string username, string password);
        void Add(User user);
        void Delete(int id);
        void Update(User user);
    }
}
