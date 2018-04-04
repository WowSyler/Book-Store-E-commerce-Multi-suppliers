using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace BookStore.Controllers
{
    public class SuppliersController : Controller
    {
        public ActionResult Index(string supp)
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                var cc = GetSupp(supp).SupplierID;
                ViewBag.suppname = GetSupp(supp).Name;
                ViewBag.suppsname = GetSupp(supp).SurName;

                
                ViewBag.totalsalebooks = data.OrderProducts.Where(v => v.SupplierID == cc).Count();
                ViewBag.addcardbooks = data.OrderDetails.Where(v => v.SupplierID == cc).Count();
                ViewBag.waitbooks = data.Products.Where(v => v.SupplierID == cc).Count();


                //satılan ve satılmıs toplam kitap
                int bb = data.Products.Where(v => v.SupplierID == cc).Count();
                var nn = data.OrderProducts.Where(v => v.SupplierID == cc).Count();
                string cv = nn.ToString();
                int vv = Convert.ToInt32(cv);
                ViewBag.totalbooks = bb + vv;



                return View();
            }
        }

        public ActionResult LoginSupp()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginSupp(FormCollection logn)
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                string userN = logn["UserName"];
                string Pass = logn["Password"];

                if (ModelState.IsValid)
                {
                    var sup = (from m in data.Suppliers
                               where (m.UserName == userN && m.Password == Pass)
                               select m).SingleOrDefault();

                    if (sup != null)
                    {

                        Session["suppname"] = sup.UserName;

                        ModelState.AddModelError("", "The user name or password provided is incorrect.");

                        return RedirectToAction("Index", "Suppliers", new { supp = Session["suppname"] });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    return View();
                }
                return View();
            }
        }

        // POST: /Supp/Register        
        [HttpPost]
        public ActionResult Register(Suppliers supp)
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                {
                  

                    if (ModelState.IsValid)
                    {
                        var rsupp = (from m in data.Suppliers
                                     where m.UserName == supp.UserName
                                     select m).Any();                   

                        if (!rsupp)
                        {
                            data.Suppliers.Add(supp);
                            Session["suppname"] = supp.UserName;
                            data.SaveChanges();
                        }
                        else
                        {
                            ModelState.AddModelError("", "Try Again.");
                            return View();
                        }
                        return RedirectToAction("Index", "Suppliers",new { supp = Session["suppname"] });

                    
                    }
                    return View(supp);
                }
            }
        }

        //cıkıs
        public ActionResult Logout()
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                Session["suppname"] = null;
                Session.Clear();
                return RedirectToAction("LoginSupp", "Suppliers");
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

        //satıcı detayları
        public ActionResult DetailSupp(string supp)
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                var rsupp = data.Suppliers.Find(GetSupp(supp).SupplierID);
                ViewBag.suppname = GetSupp(supp).Name;
                ViewBag.suppsname = GetSupp(supp).SurName;
                return View(rsupp);
            }
        }
        //satılı guncelleme
        public ActionResult UpdateSupp(string supp)
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                var rsupp = data.Suppliers.Find(GetSupp(supp).SupplierID);
                ViewBag.suppname = GetSupp(supp).Name;
                ViewBag.suppsname = GetSupp(supp).SurName;
                return View(rsupp);
            }
        }
        [HttpPost]
        public ActionResult UpdateSupp(Suppliers supp)
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                if (ModelState.IsValid)
                {
                    data.Entry(supp).State = EntityState.Modified;
                    data.SaveChanges();
                    Session["suppname"] = supp.UserName;
                    return RedirectToAction("UpdateSupp", "Suppliers", new { supp = Session["suppname"] });
                }
                return View(supp);
            }
        }

        public ActionResult ListBooks(string supp)
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                ViewBag.suppname = GetSupp(supp).Name;
                ViewBag.suppsname = GetSupp(supp).SurName;
                var sid = GetSupp(supp).SupplierID;
                var books = data.Products.Where(b => b.SupplierID == sid).OrderBy(b => b.Name).ToList();
                return View(books);
            }
        }

        public ActionResult UpdateBook(string supp, int id)
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                ViewBag.suppname = GetSupp(supp).Name;
                ViewBag.suppsname = GetSupp(supp).SurName;
                Products product = data.Products.Single(x => x.ProductID == id);
                ViewBag.catago = data.Categories.ToList();
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }
        }
        [HttpPost]
        public ActionResult UpdateBook(Products supp, HttpPostedFileBase picture1)
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                ViewBag.catago = data.Categories.ToList();
                if (ModelState.IsValid)
                {
                    if (picture1 != null)
                    {
                        WebImage img = new WebImage(picture1.InputStream);
                        FileInfo fotoinfo = new FileInfo(picture1.FileName);
                        string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                        img.Resize(250, 249);
                        img.Save("~/Content/images/products/" + newfoto);
                        supp.Picture1 = "/Content/images/products/" + newfoto;

                        data.Entry(supp).State = EntityState.Modified;
                        data.SaveChanges();
                        var bb = data.Suppliers.Where(b => b.SupplierID == supp.SupplierID).FirstOrDefault();
                        Session["suppname"] = bb.UserName;
                        return RedirectToAction("ListBooks", "Suppliers", new { supp = Session["suppname"] });
                    }
                }
                return View(supp);
            }
        }

       
        public ActionResult DeleteBook(int id ,string supp)
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                var thereisCart = db.OrderDetails.Where(b => b.ProductID == id).ToList();

                if (thereisCart == null)
                {
                    Products books = db.Products.Find(id);
                    db.Products.Remove(books);
                    db.SaveChanges();
                    return Redirect(TempData["returnURLLm"].ToString());
                }
                else
                {
                    var delete = db.OrderDetails.Where(v => v.ProductID == id).ToList();
                    db.OrderDetails.RemoveRange(delete);
                    Products books = db.Products.Find(id);
                    db.Products.Remove(books);
                    db.SaveChanges();
                    return Redirect(TempData["returnURLLm"].ToString());
                }
            }
        }

        public ActionResult DetailBook(int id, string supp)
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                ViewBag.suppname = GetSupp(supp).Name;
                ViewBag.suppsname = GetSupp(supp).SurName;
                Products product = db.Products.Find(id);
                return View(product);
            }

        }

        public ActionResult CreateBook(string supp)
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {

                ViewBag.suppname = GetSupp(supp).Name;
                ViewBag.suppsname = GetSupp(supp).SurName;

                ViewBag.catago = db.Categories.ToList();

                var ss = GetSupp(supp).SupplierID;
                ViewBag.SupplierID = db.Suppliers.Where(v=>v.SupplierID ==ss).ToList();
            return View();
            }
        }
        [HttpPost]
        public ActionResult CreateBook(HttpPostedFileBase picture1, Products book)
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                ViewBag.catago = db.Categories.ToList();       

                if (picture1 != null)
                    {
                        WebImage img = new WebImage(picture1.InputStream);
                        FileInfo fotoinfo = new FileInfo(picture1.FileName);
                        string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                        img.Resize(268, 249);
                        img.Save("~/Content/images/products/" + newfoto);
                        book.Picture1 = "/Content/images/products/"+newfoto;

                        db.Products.Add(book);
                        db.SaveChanges();
                        return RedirectToAction("ListBooks","Suppliers", new { supp = Session["suppname"] });
                    }
                ModelState.AddModelError("", "Try Again.");
                return RedirectToAction("Index","Suppliers", new { supp = Session["suppname"] });
            }
        }

        public ActionResult WaitCartBooks(string supp)
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {

                ViewBag.suppname = GetSupp(supp).Name;
                ViewBag.suppsname = GetSupp(supp).SurName;
                var sid = GetSupp(supp).SupplierID;
                var books = data.OrderDetails.Where(b => b.SupplierID == sid).OrderBy(b => b.OrderDetailsID).ToList();
                return View(books);
               
            }
        }


    }
}