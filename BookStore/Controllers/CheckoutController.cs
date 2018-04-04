using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class CheckoutController : Controller
    {

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

        // GET: Checkout
        public ActionResult Index(string user)
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                int ctvn = GetUser(user).CustomerID;

                ViewBag.ccc = (from vc in db.OrderDetails
                               where vc.CustomersID == ctvn
                               select (int?)vc.Quantity * vc.Products.Price).Sum();

                ViewBag.bname = (from vc in db.OrderDetails
                                 where vc.CustomersID == ctvn
                                 select vc.Products).ToList();

                return View();
            }
        }
        [HttpPost]
        public ActionResult PlaceOrder(FormCollection getCheckoutDetails, string user)
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                int shpID = 1;
                if (db.ShippingDetails.Count() > 0)
                {
                    shpID = db.ShippingDetails.Max(x => x.ShippingID) + 1;
                }
                int payID = 1;
                if (db.Payment.Count() > 0)
                {
                    payID = db.Payment.Max(x => x.PaymentID) + 1;
                }
                int orderID = 1;
                if (db.Order.Count() > 0)
                {
                    orderID = db.Order.Max(x => x.OrderID) + 1;
                }

                try
                {
                    ShippingDetails shpDetails = new ShippingDetails();
                    shpDetails.ShippingID = shpID;
                    shpDetails.FirstName = getCheckoutDetails["FirstName"];
                    shpDetails.LastName = getCheckoutDetails["LastName"];
                    shpDetails.Email = getCheckoutDetails["Email"];
                    shpDetails.Mobile = getCheckoutDetails["Mobile"];
                    shpDetails.Addres = getCheckoutDetails["Addres"];                  
                    shpDetails.City = getCheckoutDetails["City"];
                    shpDetails.PostCode = getCheckoutDetails["PostCode"];
                    db.ShippingDetails.Add(shpDetails);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {

                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Response.Write(string.Format("Entity türü \"{0}\" şu hatalara sahip \"{1}\" Geçerlilik hataları:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Response.Write(string.Format("- Özellik: \"{0}\", Hata: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                        }
                        Response.End();
                    }
                }
                    Payment pay = new Payment();
                    pay.PaymentID = payID;
                    pay.Type = Convert.ToString(getCheckoutDetails["PayMethod"]);
                    db.Payment.Add(pay);
                    db.SaveChanges();

                    Order o = new Order();
                    o.OrderID = orderID;
                    string userr= getCheckoutDetails["sesion"];
                    o.CustomerID = GetUser(userr).CustomerID;
                    o.PaymentID = payID;
                    o.ShippingID = shpID;
                    o.TotalPrice = Convert.ToInt32(getCheckoutDetails["totals"]);
                    o.isCompleted = true;
                    o.OrderDate = DateTime.Now;
                    db.Order.Add(o);
                    db.SaveChanges();

                
                  int ctvn = GetUser(userr).CustomerID;
                  List<OrderDetails> dd = db.OrderDetails.Where(x => x.CustomersID == ctvn).ToList();

                foreach (var od in dd)
                {
                    if(od != null)
                    {
                        OrderProducts tempOrderProduct = new OrderProducts {
                            OrderDetailsID=od.OrderDetailsID,
                            OrderID = o.OrderID,
                            ProductID = od.ProductID,
                            Price= od.Price,
                            Quantity = od.Quantity,
                            Discount= od.Discount,
                            TotalPrice = od.TotalPrice,
                            OrderDate = o.OrderDate,
                            CustomersID = od.CustomersID,
                            SupplierID = od.SupplierID
                        };

                        db.OrderProducts.Add(tempOrderProduct);
                    }
                }

                foreach (var od in dd)
                {
                    if (od != null)
                    {
                        Products tempProduct = db.Products.Where(p => p.ProductID == od.ProductID).SingleOrDefault();
                        if (tempProduct.UnitInStock >= od.Quantity)
                        {
                            tempProduct.UnitInStock -= (int)od.Quantity;
                        }

                        //eğer ürün ekliyse update yapıyor yoksa yeni ürün ekliyor 
                        // Entity.Migrations kütüphanesinin fonksiyonu
                        db.Products.AddOrUpdate(tempProduct);
                    }
                }


                if (db.SaveChanges() > 0)
                {
                    var books = (from i in db.OrderDetails where i.CustomersID == ctvn select i).ToList();
                    db.OrderDetails.RemoveRange(books);

                    db.SaveChanges();
                }                

                return RedirectToAction("Complate", "Checkout");
                }

            }
         public ActionResult Complate()
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {

                var totalorder = db.Order.Count();

                 return View(totalorder);
                
            }
        }



        } 
}