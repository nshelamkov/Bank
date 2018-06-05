using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bank.Models
{
    public class Transaction
    {
        public int ID { get; set; }
        public int CardID { get; set; }
        public DateTime OperationDate { get; set; }
        public decimal Sum { get; set; }
        public string Decription { get; set; }

        public virtual Card Card { get; set; }
    }
}