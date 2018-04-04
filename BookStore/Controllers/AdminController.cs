using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using System.Data.Entity;

namespace BookStore.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()//login index i icin bos
        {
            return View();
        }

        
        public ActionResult Logout()//otorum sonlşandırma
        {
            Session.Clear();
            return RedirectToAction("Index", "Admin");
        }


        public ActionResult Home(string Ladmin)//admın anasayfası
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                ViewBag.Aname = GetAdmin(Ladmin).name; // ekrana admının adını ve bı altdakı soyadını yazdırmak ıcın
                ViewBag.Asname = GetAdmin(Ladmin).surname;
                ViewBag.OnlineCus = HttpContext.Application["username"];//onlıne musterıleri gormek ıcın  bunun  devamı global.asax ın ıcınde  application start ve end fonksıyonu.
               

                ViewBag.TotalSupp = data.Suppliers.Count();  // musterı ve satıcıları sayıyor
                ViewBag.TotalCus = data.Customers.Count();

                ViewBag.totalbooks = data.Products.GroupBy(test => test.Name).Select(grp => grp.ToList()).Count();//product tablasundakı urunlerıu sayıyor.
                ViewBag.totalsalebooks = data.OrderProducts.Select(m=>m.Quantity).Sum();// satılan urunlerı sayıyor
                ViewBag.addcarddifbooks = data.OrderDetails.Count(); // sepetdeki urunlerıu sayıyor
                ViewBag.waitbooks = data.Products.Select(b => b.UnitInStock).Sum(); // urunler tablosundakı urunlerı stoklarını topluyor
                ViewBag.addcardbooks = data.OrderDetails.Select(b=>b.Quantity).Sum();// cartdakı urunlerı mıktarını topluyor  
               
                //satılan ve satılmıs toplam kitap
                int unitstok = data.Products.Select(b => b.UnitInStock).Sum(); // urunler tablosundakı urunlerı stoklarını topluyor
                var orderq = data.OrderProducts.Select(m => m.Quantity).Sum(); // satılanlar tablosundakı urunlerı mıktarını topluyor
                string total = orderq.ToString();
                int itotal = Convert.ToInt32(total);
                ViewBag.totalbook = unitstok + itotal; // ustdeki ikisi topluyup htmle atıyoruz
                return View();
            }
        }

        public Admin GetAdmin(string _admn)//getuserın aynı sadece admın bılgısı ıcın
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                var radmn = (from a in data.Admin
                             where a.username == _admn
                             select a).FirstOrDefault();
                return radmn;
            }
        }

        [HttpPost]
            public ActionResult Login(Admin admin)//login işlemi için httpPost fonsitoynu 
                //htmlden admin modeli ile aldıgı  bilgileri  username pass ile databasede ara ve bilgiler doyru ise login işleminin gercekleştirir  usernamının sessıon olarak atar
    
               //ve ana admin sayfasına yonlendırır login olursa. olmazsa aysı sayfayı donderir. login olmasa hata donderır.
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
                {
                    if (ModelState.IsValid)
                    {
                    var admn = (from m in data.Admin
                               where (m.username == admin.username && m.password == admin.password)
                               select m).Any();

                         if (admn)
                           {
                        var loginInfo = data.Admin.Where(x => x.username == admin.username && x.password == admin.password).FirstOrDefault();

                        Session["admin"] = loginInfo.username;     

                        return RedirectToAction("Home", "Admin", new { Ladmin = Session["admin"] });
                         }
                    }
                    else
                     {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    return View("Index");
                       }              
                return RedirectToAction("Index", "Admin");
                }
           
            }

        public ActionResult DetailAdmin(string Ladmin)// admın bilgılerını detail olarak model den atar htmlle 
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                var radd = data.Admin.Find(GetAdmin(Ladmin).adminID);
                ViewBag.suppname = GetAdmin(Ladmin).name;
                ViewBag.suppsname = GetAdmin(Ladmin).surname;
                return View(radd);
            }
            
        }

        public ActionResult ListSupp()//satıların hepsini model olarak atar
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {

                var model = data.Suppliers.ToList();
                return View(model);
            }
        }

        public ActionResult DeleteSupp(int id, string Ladmin)//gonderılen ıd degferlı satıcıyı sıler ve sil tusunun basıldıgı sayfaya gerı yondendırır.
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                var books = db.Products.Where(c => c.SupplierID == id).ToList();
                if (books != null)
                {
                    db.Products.RemoveRange(books);
                    Suppliers suppl = db.Suppliers.Find(id);
                    db.Suppliers.Remove(suppl);
                    db.SaveChanges();
                    return Redirect(TempData["returnsuplis"].ToString());
                }
                else
                {
                    Suppliers suppl = db.Suppliers.Find(id);
                    db.Suppliers.Remove(suppl);
                    db.SaveChanges();
                    return Redirect(TempData["returnsuplis"].ToString());
                }
            }
        }
        public ActionResult UpdateSupp(string Ladmin, int id)//sup bilgerini guncelemek ıcın  gonderılen ıd degerlı sup bılgılerı ıle sayfayı acar
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
               
                Suppliers suppl = data.Suppliers.Single(x => x.SupplierID == id);
                if (suppl == null)
                {
                    return HttpNotFound();
                }
                return View(suppl);
            }
        }
        [HttpPost]
        public ActionResult UpdateSupp(Suppliers supp)//sup bilgilerini degıstırdıkten sonra save butununa bastıgımız yenı degerlerı buraya gonderırır ve kayeder. lıstsup sayfasına gonmderır
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                if (ModelState.IsValid)
                {
                    data.Entry(supp).State = EntityState.Modified;
                    data.SaveChanges();
                   
                    return RedirectToAction("ListSupp", "Admin", new { Ladmin = Session["admin"] });
                }
                return View(supp);
            }
        }


        public ActionResult ListCus()//musterı lısteler model olarak
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {

                var model = data.Customers.ToList();
                return View(model);
            }
        }

        public ActionResult DeleteCus(int id, string Ladmin)//gonderılen ıd degferlı musterıyı sıler ve sil tusunun basıldıgı sayfaya gerı yondendırır.
        {
            using (bookStoreDBEntities db = new bookStoreDBEntities())
            {
                Customers cust = db.Customers.Find(id);
                db.Customers.Remove(cust);
                db.SaveChanges();
                return Redirect(TempData["retlist"].ToString());
            }
        }
        public ActionResult UpdateCus(string Ladmin, int id)//cus bilgerini guncelemek ıcın  gonderılen ıd degerlı sup bılgılerı ıle sayfayı acar
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {

                Customers supp = data.Customers.Single(x => x.CustomerID == id);
                if (supp == null)
                {
                    return HttpNotFound();
                }
                return View(supp);
            }
        }
        [HttpPost]
        public ActionResult UpdateCus(Customers cuss)//cus bilgilerini degıstırdıkten sonra save butununa bastıgımız yenı degerlerı buraya gonderırır ve kayeder. lıstsup sayfasına gonmderır
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                if (ModelState.IsValid)
                {
                    data.Entry(cuss).State = EntityState.Modified;
                    data.SaveChanges();
                   
                    return RedirectToAction("ListCus", "Admin", new { Ladmin = Session["admin"] });
                }
                return View(cuss);
            }
        }
    }
}