using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Credentials
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public string CardNumber { get; set; }
        public int expMonth{ get; set; }
        public int expYear { get; set; }
        public int CVV { get; set; }
        public DateTime CreatedAt { get; set; }

        public Credentials() { }
        public Credentials(int userid, string cardnum, int month, int year, int cvv, DateTime createdAt)
        {
            UserId = userid;
            CardNumber = cardnum;
            expMonth = month;
            expYear = year;
            CVV = cvv;
            CreatedAt = createdAt;
        }
    }
}
