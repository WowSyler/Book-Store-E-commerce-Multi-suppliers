using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.ViewModel;

namespace BookStore.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index(string user)//vıew model dir yani 3 farklı tobloyu birleştirip  tek bir model olusturur.
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                int Cust = GetUser(user).CustomerID;
                List<Cart> newcart = new List<Cart>();
                var cartorder = (from ordd in db.OrderDetails// joın ıslemi ile 3 tabloyu bırlestırıyoruz 
                                 join pro in db.Products
                                 on ordd.ProductID equals pro.ProductID
                                 join sup in db.Suppliers
                                 on pro.SupplierID equals sup.SupplierID
                                 select new
                                 {
                                   //orderteaisl 
                                   ordd.ProductID,
                                   ordd.Price,
                                   ordd.Quantity,
                                   ordd.TotalPrice,
                                   ordd.OrderDate,
                                   ordd.CustomersID,
                                   ordd.SupplierID,     

                                     //book
                                     pro.Name,
                                     pro.Picture1,

                                     //suppliıesr
                                     sup.UserName

                                 }).Where(v => v.CustomersID == Cust).ToList();//musterının ıd sıne gore 

                foreach (var item in cartorder) //ve 3 tablodan bılgılerı cekıp   cart modeline gore dolduruyoruz.
                {
                    Cart sl = new Cart();
                    sl.ProductID = item.ProductID;
                    sl.Price = item.Price;
                    sl.Quantity = item.Quantity;
                    sl.TotalPrice = item.TotalPrice;
                    sl.OrderDate = item.OrderDate;
                    sl.CustomersID = item.CustomersID;
                    sl.SupplierID = item.SupplierID;
                    
                    sl.Name = item.Name;
                    sl.Picture1 = item.Picture1;

                    sl.UserName = item.UserName;
                    //saleslist ilk acılan liste en basta
                    newcart.Add(sl);
                }

                //total cart sum
                ViewBag.ccc = (from Orderrow in db.OrderDetails
                               where Orderrow.CustomersID == Cust
                               select (int?)Orderrow.Quantity * Orderrow.Products.Price).Sum();

                return View(newcart);

            }


        }

        public ActionResult Remove(int id ,string user)// cart ıcınde  urun sılmemızı saglıyor.
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                int Cust = GetUser(user).CustomerID;
                var books = (from i in db.OrderDetails where i.ProductID == id && i.CustomersID == Cust select i).FirstOrDefault();          
                db.OrderDetails.Remove(books);
                db.SaveChanges();
                return Redirect(TempData["returnURLL"].ToString());

            }
        }
       /*          sil bunu  bu yok asjhgdasjhdg 
        [HttpPost]
        public ActionResult ProcedToCheckout(FormCollection formcoll)
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {                      
            return RedirectToAction("Index", "CheckOut");
            }

        }
        */

        public Customers GetUser(string _usrName)
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                var rcust = (from c in data.Customers
                             where c.UserName == _usrName
                             select c).FirstOrDefault();
                return rcust;
            }
        }
    }
}