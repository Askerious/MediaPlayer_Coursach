using Data.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.SqlServer
{
    public class UserRepository : IUserRepository
    {
        private readonly MediaDbContext _ctx;
        public UserRepository(MediaDbContext ctx) => _ctx = ctx;

        public List<User> GetAll() => _ctx.Users.ToList();
        public User GetById(int id) => _ctx.Users.Find(id);

        public bool CheckPassword(string username, string password)
        {
            User user = _ctx.Users.Where(p => p.Username == username).First();
            if (user == null) return false;

            if (user.Password == password)
                return true;
            else return false;
        }

        public void Add(User user)
        {
            _ctx.Users.Add(user);
            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            var ent = _ctx.Users.Find(id);
            if (ent == null) return;

            _ctx.Users.Remove(ent);
            _ctx.SaveChanges();
        }

        public void Update(User user)
        {
            _ctx.Users.Update(user);
            _ctx.SaveChanges();
        }
    }
}
