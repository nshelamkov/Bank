using Bank.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bank.DAL
{
    public class BankInitializer:DropCreateDatabaseIfModelChanges<BankContext>
    {
        protected override void Seed(BankContext context)
        {
            var users = new List<User>
            {
            new User{ Name="Nick", Surname="Shelamkov", Email="n.shelamkov@ukr.net", Password="123" },
            new User{ Name="Leo", Surname="Messi", Email="messi@barcelona.com", Password="123"  },
            new User{ Name="admin", Surname="admin", Email="admin@ukr.net", Password="admin" }
            
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();

            var cards = new List<Card>
            {
            new Card{ CardNumber="1234123412341234", Pin="1111", Balance=100, UserID=1  },
            new Card{ CardNumber="4321432143214321", Pin="1111", Balance=200, UserID=1  },
            new Card{ CardNumber="6789567867895678", Pin="1111", Balance=300, UserID=2  },
            new Card{ CardNumber="3456345634563456", Pin="1111", Balance=400, UserID=1  },
            new Card{ CardNumber="7890789078907890", Pin="1111", Balance=100, UserID=3  },
            new Card{ CardNumber="9876987698769876", Pin="1111", Balance=500, UserID=2  },
            };

            cards.ForEach(c => context.Cards.Add(c));
            context.SaveChanges();

            var transactions = new List<Transaction> {
                new Transaction{ OperationDate=DateTime.Parse("2017-09-01"), Decription="111", Sum=50, CardID=1 },
                new Transaction{ OperationDate=DateTime.Parse("2017-09-01"), Decription="222", Sum=50, CardID=2 },
                new Transaction{ OperationDate=DateTime.Parse("2017-09-01"), Decription="333", Sum=50, CardID=1 },
                new Transaction{ OperationDate=DateTime.Parse("2017-09-01"), Decription="444", Sum=50, CardID=3 },
                new Transaction{ OperationDate=DateTime.Parse("2017-09-01"), Decription="555", Sum=50, CardID=1 },
                new Transaction{ OperationDate=DateTime.Parse("2017-09-01"), Decription="666", Sum=50, CardID=2 },
                new Transaction{ OperationDate=DateTime.Parse("2017-09-01"), Decription="777", Sum=50, CardID=1 },
            };

            transactions.ForEach(t => context.Transactions.Add(t));
            context.SaveChanges();
        }
    }
}