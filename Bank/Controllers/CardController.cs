using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bank.DAL;
using Bank.Models;

namespace Bank.Controllers
{
    [Authorize]
    public class CardController : Controller
    {
        private BankContext db = new BankContext();

        // GET: Card
        
        public ActionResult Index()
        {

            var cards = db.Cards.Include(c => c.User).Where(p=>p.User.Name==User.Identity.Name);
            foreach (var card in cards.ToList())
            {
                card.Balance = card.Transactions.Sum(p => p.Sum);
            }
            return View(cards.ToList());
        }

        // GET: Card/Details/5
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Card card = db.Cards.Find(id);
            card.Balance = card.Transactions.Sum(p => p.Sum);
            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        
        private string RandomCardNumber(int length) {
            var random = new Random();
            string s = String.Empty;
            for (int i = 0; i < length; i++)
            {
                s = String.Concat(s, random.Next(10).ToString());
            }
            return s;
        }

        // GET: Card/Create
        
        public ActionResult Create()
        {
            //ViewBag.UserID = new SelectList(db.Users, "Id", "Email");
            //ViewBag.CardNumber = RandomCardNumber(16);
            return View();
        }

        // POST: Card/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public ActionResult Create([Bind(Include = "ID,CardNumber,Pin,Balance,UserID")] Card card)
        {
            if (ModelState.IsValid)
            {
                card.CardNumber= RandomCardNumber(16);
                card.Balance = 0;
                card.UserID = db.Users.Where(p => p.Name == User.Identity.Name).Select(p => p.Id).FirstOrDefault();
                db.Cards.Add(card);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "Id", "Email", card.UserID);
            return View(card);
        }

        // GET: Card/Edit/5
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "Id", "Email", card.UserID);
            return View(card);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CardNumber,Pin,Balance,UserID")] Card card)
        {
            if (ModelState.IsValid)
            {
                db.Entry(card).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "Id", "Email", card.UserID);
            return View(card);
        }

        // GET: Card/Delete/5
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        // POST: Card/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Card card = db.Cards.Find(id);
            db.Cards.Remove(card);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        
        public ActionResult AddMoney(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMoney(AddMoneyModel model, int id)
        {
            Card card = db.Cards.Find(id);
            
            if (model.UserTypePin==card.Pin)
            {
                Transaction tr = new Transaction();
                tr.OperationDate = DateTime.Now;
                tr.CardID = id;
                tr.Sum = model.Sum;
                tr.Decription = model.Description;
               
                db.Transactions.Add(tr);
                db.SaveChanges();
                card.Balance = card.Transactions.Sum(p => p.Sum);
                return RedirectToAction("Details", "Card", new { id = tr.CardID });
            }
            return View(model);

        }

        
        public ActionResult WriteOffCashFromCart(int id)
        {
            Card card = db.Cards.Find(id);
            ViewBag.Balance = card.Transactions.Sum(p => p.Sum);
            return View();
        }

        [HttpPost]
        public ActionResult WriteOffCashFromCart(WriteOffMoneyModel model, int id)
        {
            Card card = db.Cards.Find(id);
            model.Balance = card.Transactions.Sum(p => p.Sum);

            if (model.UserTypePin == card.Pin)
            {
                if (model.Sum<= model.Balance)
                {
                    Transaction tr = new Transaction();
                    tr.OperationDate = DateTime.Now;
                    tr.CardID = id;
                    tr.Sum = -model.Sum;
                    tr.Decription = model.Description;

                    db.Transactions.Add(tr);
                    db.SaveChanges();
                    card.Balance = card.Transactions.Sum(p => p.Sum);
                    return RedirectToAction("Details", "Card", new { id = tr.CardID });
                }
                return View(model);
            }
            return View(model);

        }

        
        public ActionResult TransferMoney(int id)
        {
            Card card = db.Cards.Find(id);
            ViewBag.toCardID = new SelectList(db.Cards, "ID", "CardNumber");
            return View();
        }

        [HttpPost]
        public ActionResult TransferMoney(TransferMoneyModel model, int id)
        {
            Card card = db.Cards.Find(id);
            model.Balance = card.Transactions.Sum(p => p.Sum);
            ViewBag.toCardID = new SelectList(db.Cards, "ID", "CardNumber", model.toCardID);
            if (model.UserTypePin == card.Pin)
            {
                if (model.Sum <= model.Balance)
                {
                    Transaction tr = new Transaction();
                    tr.OperationDate = DateTime.Now;
                    tr.CardID = id;
                    tr.Sum = -model.Sum;
                    tr.Decription = model.Description;

                    db.Transactions.Add(tr);
                    db.SaveChanges();
                    card.Balance = card.Transactions.Sum(p => p.Sum);

                    Transaction tr1 = new Transaction();
                    tr1.OperationDate = DateTime.Now;
                    tr1.CardID = model.toCardID;
                    tr1.Sum = model.Sum;
                    tr1.Decription = "Перевод с карты "+ tr.Card.CardNumber;

                    db.Transactions.Add(tr1);
                    db.SaveChanges();
                    Card cardTo = db.Cards.Find(id);
                    card.Balance = card.Transactions.Sum(p => p.Sum);
                    
                    return RedirectToAction("Details", "Card", new { id = tr.CardID });
                }
                return View(model);
            }
            return View(model);

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
