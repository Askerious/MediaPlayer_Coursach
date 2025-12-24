using Data.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.SqlServer
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly MediaDbContext _ctx;
        public PaymentRepository(MediaDbContext ctx) => _ctx = ctx;

        public List<Credentials> GetAll() => _ctx.Credentials.AsQueryable().ToList();
        public Credentials GetByUserId(int id) => _ctx.Credentials.Where(p => p.UserId == id).FirstOrDefault();
        public Credentials GetById(int id) => _ctx.Credentials.Find(id);

        public void Add(Credentials card)
        {
            _ctx.Credentials.Add(card);
            _ctx.SaveChanges();
        }
    }
}
