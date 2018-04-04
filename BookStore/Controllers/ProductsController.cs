using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity;
using Microsoft.ApplicationInsights;

namespace BookStore.Controllers
{
    public class ProductsController : Controller
    {
        private TelemetryClient telemetry = new TelemetryClient();
        // GET: Products
        public ActionResult Index(int? page)
        {
           
                using (bookStoreDBEntities data = new bookStoreDBEntities())
                {

                    ViewBag.cata = data.Categories.ToList().OrderBy(x => x.Name);
                    ViewBag.rec = data.Products.Where(b => b.active == true).OrderByDescending(b => b.Price).Take(4).ToList();
                    ViewBag.rec2 = data.Products.Where(b => b.active == true).OrderByDescending(b => b.CategoryID).Take(4).ToList();

                    int Page1 = page ?? 1;

              

                var urunler = data.Products.Where(b => b.active == true).Distinct().OrderByDescending(m => m.UnitInStock).ToPagedList<Products>(Page1, 12);

                telemetry.TrackPageView("ProductPage");
                return View(urunler);
                }
            
        }
       

        public ActionResult Details(int id , string bname)
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                ViewBag.cata = data.Categories.ToList().OrderBy(x => x.Name);
                ViewBag.rec = data.Products.Where(b => b.active == true).OrderByDescending(b => b.Price).Take(4).ToList();
                ViewBag.rec2 = data.Products.Where(b => b.active == true).OrderByDescending(b => b.CategoryID).Take(4).ToList();
                ViewBag.SameBooks = data.Products.Where(b => b.active == true).Where(b => b.Name == bname).ToList();
                ViewBag.SameBookcount = data.Products.Where(b => b.active == true).Where(b => b.Name == bname).Count();


                ViewBag.comments = data.Comments.Where(b => b.active == true).Where(v => v.ProductsID == id).ToList();
                ViewBag.commentSum = data.Comments.Where(b => b.active == true).Where(v => v.ProductsID == id).Select(n=>n.commentID).Count();
                ViewBag.callpid = id;
                var prod = data.Products.Find(id);

                int suppforid = data.Products.Where(b => b.ProductID == id).Select(b => b.SupplierID).FirstOrDefault();
                ViewBag.suppinfo = data.Suppliers.Where(n => n.SupplierID == suppforid).FirstOrDefault();

                telemetry.TrackPageView("DetailPage");
                return View(prod);
            }

        }

        //for the comment 
        [HttpPost]
        public ActionResult Details(int id, string name , string text)
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                ViewBag.cata = data.Categories.ToList().OrderBy(x => x.Name);
                ViewBag.rec = data.Products.Where(b => b.active == true).OrderByDescending(b => b.Price).Take(4).ToList();
                ViewBag.rec2 = data.Products.Where(b => b.active == true).OrderByDescending(b => b.CategoryID).Take(4).ToList();
                ViewBag.comments = data.Comments.Where(b => b.active == true).Where(v => v.ProductsID == id).ToList();

                Comments newcomment = new Comments();
                newcomment.name = name;
                newcomment.text = text;
                newcomment.ProductsID = id;
                newcomment.Commentdate = DateTime.Now;
                newcomment.active = false;
                data.Comments.Add(newcomment);
                data.SaveChanges();

                return RedirectToAction("Details");
            }
        }

            //site ici arama 
            public ActionResult Search(string product, int? page)
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                ViewBag.cata = data.Categories.ToList().OrderBy(x => x.Name);
                ViewBag.products = data.Products.Where(b => b.active == true).OrderBy(u => u.ProductID).ToList();
                ViewBag.rec = data.Products.Where(b => b.active == true).OrderByDescending(b => b.Price).Take(4).ToList();
                ViewBag.rec2 = data.Products.Where(b => b.active == true).OrderByDescending(b => b.CategoryID).Take(4).ToList();

                List<Products> productss;
                if (!string.IsNullOrEmpty(product))
                {
                    productss = data.Products.Where(b => b.active == true).Where(x => x.Name.Contains(product)).ToList();
                    
                }
                else
                {
                
               productss = data.Products.Where(b => b.active == true).ToList();
                }
               
                return View("Index", productss.ToPagedList(page ?? 1, 8));
            }
        }


        //categori menusu
        public ActionResult Catagori(int catID, int? page)
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                ViewBag.cata = data.Categories.ToList().OrderBy(x => x.Name);
                
                ViewBag.rec = data.Products.Where(b => b.active == true).OrderByDescending(b => b.Price).Take(4).ToList();
                ViewBag.rec2 = data.Products.Where(b => b.active == true).OrderByDescending(b => b.CategoryID).Take(4).ToList();

                ViewBag.catid = catID;
                var prods = data.Products.Where(b => b.active == true).Where(x => x.Categories.CategoryID == catID).ToList();

                ViewBag.categoriadi = data.Categories.Where(v => v.CategoryID == catID).Select(n => n.Name).FirstOrDefault();


                return View( prods.ToPagedList(page ?? 1, 8));
                
            }
        }

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
        //ADD TO CART
        public ActionResult AddToCart(int id, int qty, string user)
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                telemetry.TrackEvent("addcartbook");
                int itemQ = data.Products.Where(b => b.ProductID == id).Select(v => v.UnitInStock).FirstOrDefault();
              
                OrderDetails OD = new OrderDetails();
                int ctvn = GetUser(user).CustomerID;
                var books = (from i in data.OrderDetails where i.ProductID == id && i.CustomersID == ctvn select i).FirstOrDefault();

               
                    if (books == null)
                    {
                        OD.ProductID = id;
                        OD.CustomersID = GetUser(user).CustomerID;
                        decimal price = data.Products.Find(id).Price;
                        OD.Quantity = qty;
                        OD.Price = price;
                        OD.TotalPrice = qty * price;
                        OD.Products = data.Products.Find(id);
                        OD.SupplierID = data.Products.Find(id).SupplierID;
                        data.OrderDetails.Add(OD);
                    }
                    else
                    {
                        if (itemQ > books.Quantity)
                        {
                            books.Quantity += 1;
                        decimal price = data.Products.Find(id).Price;
                        books.TotalPrice = books.Quantity * price;
                        }
                        else
                        {
                        return Redirect(TempData["returnURL"].ToString());
                        }
                }

                
               
                data.SaveChanges();
                    return Redirect(TempData["returnURL"].ToString());                
            }
        }
     
    }
}