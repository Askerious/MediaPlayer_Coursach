using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IPaymentRepository
    {
        void Add(Credentials card);
        Credentials GetByUserId(int userId);
        Credentials GetById(int id);
        List<Credentials> GetAll();
    }
}
