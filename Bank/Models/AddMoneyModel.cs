using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bank.Models
{
    public class AddMoneyModel
    {
        public decimal Sum { get; set; }
        public string Description { get; set; }
        public string CardPin { get; set; }
        [Compare("CardPin", ErrorMessage = "Неверный пин-код")]       
        public string UserTypePin { get; set; }
    }
}