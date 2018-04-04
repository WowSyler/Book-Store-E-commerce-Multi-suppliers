using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class AdminSiteController : Controller
    {
        // GET: AdminSite
        public ActionResult ListBooks()//kitapları lısteler ısımlerını gore iste
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                var model = data.Products.OrderBy(b=>b.Name).ToList();
                return View(model);
            }
        }
        public ActionResult DeleteBook(int id, string Ladmin)// gonderılen  ıd degerlı kıtabı sıler. sılınmek ısteden kıtap  bır musterının sepetınde varmı yokmu bakar
            //yok kıtabı sıler hemen ve kayeder.  varsa ılk cart tablosundan sıler sonra urunlerden sıler ve kayeder. 
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {

                var cartaKitapvar = db.OrderDetails.Where(b => b.ProductID == id).ToList();

                if (cartaKitapvar == null)
                { 
                Products books = db.Products.Find(id);
                db.Products.Remove(books);
                db.SaveChanges();
                return Redirect(TempData["returnlistbook"].ToString());
                }
                else
                {
                   var delete = db.OrderDetails.Where(v=>v.ProductID == id).ToList();
                    db.OrderDetails.RemoveRange(delete);
                    Products books = db.Products.Find(id);
                    db.Products.Remove(books);
                    db.SaveChanges();
                    return Redirect(TempData["returnlistbook"].ToString());
                }



            }
        }
        public ActionResult UpdateBook(string Ladmin, int id)//gonderılen ıd degerlı kıtapın update  html ı acar  update edıcek bılgılerı model olarask html e gonderır
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                ViewBag.catago = data.Categories.ToList();
                Products books = data.Products.Single(x => x.ProductID == id);
                if (books == null)
                {
                    return HttpNotFound();
                }
                return View(books);
            }
        }
        [HttpPost]//bu fonsıyon ıkı deger alır  products modeli ve HttpPostedFileBase (resim dosyası)
                  //resım varsa işlem yapar 
                  //resmı alır WebImage img = new WebImage(picture1.InputStream);          yenı ısım verır resme string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension; 
                  //yenıden boyutlandırır  img.Resize(268, 249);    ve resmı klasore ve resmın yolunuda databse kayeder.
        public ActionResult UpdateBook(Products books, HttpPostedFileBase picture1)
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
                        img.Resize(268, 249);
                        img.Save("~/Content/images/products/" + newfoto);
                        books.Picture1 = "/Content/images/products/" + newfoto;

                        data.Entry(books).State = EntityState.Modified;
                        data.SaveChanges();


                        return RedirectToAction("ListBooks", "AdminSite", new { Ladmin = Session["admin"] });
                    }

                }
                return View(books);
            }
        }

        public ActionResult Promo()//promo lısteler
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                var model = data.Promo.ToList();
                ViewBag.catago = data.Categories.ToList();
                return View(model);
            }
        }
        public ActionResult DeletePromo(int id, string Ladmin)// gelen ıd degerlı promoyu sıler 
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                Promo books = db.Promo.Find(id);
                db.Promo.Remove(books);
                db.SaveChanges();
                return Redirect(TempData["promourl"].ToString());
            }
        }      
        public ActionResult CreatePromo(string Ladmin)// yenı promo olursutrmak ıcın  htmlı acar 
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                

                ViewBag.catago = db.Categories.ToList();
  
                return View();
            }
        }
        [HttpPost]//yukarda acılan html sayfası ıcıne gırınlen degerlerı  bu fonsyıonu gonderır resım ıslemlerı yıne ustdekıyle aynı
        public ActionResult CreatePromo(HttpPostedFileBase link, Promo pro)
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                ViewBag.catago = db.Categories.ToList();

                if (link != null)
                {
                    WebImage img = new WebImage(link.InputStream);
                    FileInfo fotoinfo = new FileInfo(link.FileName);
                    string newfoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(268, 249);
                    img.Save("~/Content/images/Promo/"+newfoto);
                    pro.link = "/Content/images/Promo/"+newfoto;

                    db.Promo.Add(pro);
                    db.SaveChanges();
                    return RedirectToAction("Promo", "AdminSite", new { Ladmin = Session["admin"] });
                }
                ModelState.AddModelError("", "Try Again.");
                return RedirectToAction("Home", "Admin", new { Ladmin = Session["admin"] });
            }
        }
        public ActionResult Categories()// categorıu lısteler  model olarak htmle atar 
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                var model = db.Categories.ToList();
                return View(model);
            }
        }
        public ActionResult Deletecata(int id, string Ladmin)// gonderılen ıd degerlı catayı sıler.
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                Categories catog = db.Categories.Find(id);
                db.Categories.Remove(catog);
                db.SaveChanges();
                return Redirect(TempData["reee"].ToString());
            }
        }
        public ActionResult UpdateCata(string Ladmin, int id)//gonderılen ıd degerlı catayı acar  
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                var upcatog = data.Categories.Find(id);
               
                return View(upcatog);
            }
        }
        [HttpPost]
        public ActionResult UpdateCata(Categories cat) // yukarda actımız catanın bılgılerını degıstırırıp yenıden gonderırı ve guncelelme ıslemını yapar
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                if (ModelState.IsValid)
                {
                    data.Entry(cat).State = EntityState.Modified;
                    data.SaveChanges();
                    
                    return RedirectToAction("Categories", "AdminSite", new { Ladmin = Session["admin"] });
                }
                return View(cat);
            }
        }
        public ActionResult CreateCata(string Ladmin)// catagorı olursuturmna sayfasını acar 
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {

                return View();
            }
        }
        [HttpPost]//sayfayı doldurduktan sonra model olarak gonderırız bu fonsıyona ve yenı catagorıyı kayeder.
        public ActionResult CreateCata(Categories catt)
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                db.Categories.Add(catt);
                db.SaveChanges();
                return RedirectToAction("Categories", "AdminSite", new { Ladmin = Session["admin"] });
            }
        }



        //kitap oanylama kısmı 

        public ActionResult ListValidBooks() // yenı eklenen sıtede henuz yayınlanmaya kıtapları gosterırır
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                var model = db.Products.Where(n => n.active ==false  ).ToList();

                return View(model);
            }
        }
        
        public ActionResult ListValidBook(int id, string Ladmin)// ona6y vermek ıstenen kıtap ıs sıne gone  kitabın actıvce kolon degerını true yapar ve kitap sitede listelenır
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                    Products book = db.Products.Find(id);
                    book.active = true;
                    db.Products.AddOrUpdate(book);
                    db.SaveChanges();
                    return Redirect(TempData["validd"].ToString());
                
                
            }
        }
        public ActionResult DeleteValidBook(int id, string Ladmin)//onay verılmek ıstenmeye kıtabı sıler.
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                var bookdelete = db.Products.Find(id);
                db.Products.Remove(bookdelete);
                db.SaveChanges();
                return Redirect(TempData["validd"].ToString());


            }
        }

        //bu altdakı  ucude bu ustdekı ucunun aynısı 

        public ActionResult ListValidCommets()
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                var model = db.Comments.Where(n => n.active == false).ToList();

                return View(model);
            }
        }

        public ActionResult ListValidComment(int id, string Ladmin)
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                Comments comm = db.Comments.Find(id);
                comm.active = true;
                db.Comments.AddOrUpdate(comm);
                db.SaveChanges();
                return Redirect(TempData["commet"].ToString());
            }
        }
        public ActionResult DeleteValidCommet(int id, string Ladmin)
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                var commentdelete = db.Comments.Find(id);
                db.Comments.Remove(commentdelete);
                db.SaveChanges();
                return Redirect(TempData["commet"].ToString());
            }
        }
      
           
    }
}