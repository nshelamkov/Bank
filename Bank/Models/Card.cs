using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bank.Models
{
    public class Card
    {
        public int ID { get; set; }

        public string CardNumber { get; set; }

        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Пин-код должен быть только из цифр")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Должно быть 4 цифры")]
        public string Pin { get; set; }
        public decimal Balance { get; set; } = 0;        
        public int UserID { get; set; }

        public virtual List<Transaction> Transactions { get; set; }
        public virtual User User { get; set; }
    }
}