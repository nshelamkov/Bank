using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bank.Models
{
    public class WriteOffMoneyModel
    {
        [Compare("Balance", ErrorMessage = "Недостаточно средств")]
        public decimal Sum { get; set; }
        public string Description { get; set; }
        public string CardPin { get; set; }
        [Compare("CardPin", ErrorMessage = "Неверный пин-код")]
        public string UserTypePin { get; set; }
        public decimal Balance { get; set; }
    }
}