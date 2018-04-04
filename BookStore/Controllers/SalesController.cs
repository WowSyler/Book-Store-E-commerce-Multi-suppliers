using System;
using BookStore.Models;
using BookStore.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class SalesController : Controller
    {
        // GET: Sales
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SalesSup(string supp)
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                int supid = GetSupp(supp).SupplierID;
                List<Sales> saleslist = new List<Sales>();
                var salesdate = (from cus in db.Customers
                                 join ord in db.OrderProducts 
                                 
                                 on cus.CustomerID equals ord.CustomersID
                                 join pro in db.Products
                                 on ord.ProductID equals pro.ProductID
                                 select new
                                 {
                                     //customer
                                     cus.FirstName,
                                     cus.LastName,
                                     cus.Email,
                                     cus.Phone,
                                     cus.Country,
                                     cus.City,
                                     cus.Address,

                                     //orderproducts
                                     ord.OrderID,
                                     ord.ProductID,
                                     ord.Price,
                                     ord.Quantity,
                                     ord.TotalPrice,
                                     ord.OrderDate,
                                     ord.SupplierID,

                                     //book
                                     pro.Name,
                                     pro.Picture1

                                 }).Where(v=>v.SupplierID == supid).ToList();

                foreach(var item in salesdate)
                {
                    Sales sl = new Sales();
                    sl.FirstName = item.FirstName;
                    sl.LastName = item.LastName;
                    sl.Email = item.Email;
                    sl.Phone = item.Phone;
                    sl.Country = item.Country;
                    sl.City = item.City;
                    sl.Address = item.Address;


                    sl.OrderID = item.OrderID;
                    sl.ProductID = item.ProductID;
                    sl.Price = item.Price;
                    sl.Quantity = item.Quantity;
                    sl.TotalPrice = item.TotalPrice;
                    sl.OrderDate = item.OrderDate;


                    sl.Name = item.Name;
                    sl.Picture1 = item.Picture1;
                    //saleslist ilk acılan liste en basta
                    saleslist.Add(sl);
                }


                return View(saleslist);

            }
        }

        //satıcı bılgısı cekme
        public Suppliers GetSupp(string _suppn)
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                var rsupp = (from c in data.Suppliers
                             where c.UserName == _suppn
                             select c).FirstOrDefault();
                return rsupp;
            }
        }
    }
}