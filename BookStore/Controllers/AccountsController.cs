using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BookStore.Controllers
{
    public class AccountsController : Controller
    {
        
        public ActionResult Index()//bos fonsýon sadece index htmlini aciyor
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
            
                return View();               
            }
        }

        //LOG IN
        [HttpPost]
        public ActionResult Index(FormCollection loginn )  //log'n işlemi için httpPost fonsitoynu 
                //htmlden FormCollection ile aldıgı form bilgilerini  username pass ile databasede ara ve bilgiler doyru ise login işleminin gercekleştirir
                //ve ana sayfaya yonlendırır login olursa. olmazsa aysı sayfayı donderir.
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
              

                string usrName = loginn["UserName"];
                string Pass = loginn["Password"];

                if (ModelState.IsValid)
                {
                    var cust = (from m in data.Customers
                                where (m.UserName == usrName && m.Password == Pass)
                                select m).SingleOrDefault();

                    if (cust != null)
                    {
                       
                        Session["username"] = cust.UserName;                        
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                        
                        return RedirectToAction("Index", "Home", new { user = Session["username"] });
                    }
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    return View();
                }
                return View();
            }
        }

        //LOG OUT
        public ActionResult Logout()//oturumu sonlandırır sessiınonu siler.
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                Session["username"] = null;

                Session.Clear();
                return RedirectToAction("Index", "Home");
            }
        }

        //Register
        [HttpPost]
        public ActionResult Register(Customers cust)//html den customer modeli ile bilgileri alır ve alınan username ile database deki usernameleri karsılastırır. aynı user name var ıse kayıt yapmaz ve hata dondurur 
            //eger yok ıse yenı musterıyı database kayeder.    ana sayfaya yonlendırır ve username ınıde seessiınon olarak atar ve onuda gonderır.
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                
                    if (ModelState.IsValid)
                    {
                        var rcust = (from m in data.Customers
                                     where m.UserName == cust.UserName
                                     select m).Any();
                        if (!rcust)
                        {
                        data.Customers.Add(cust);                     
                        Session["username"] = cust.UserName;
                        data.SaveChanges();                      
                        }
                        else
                        {
                            ModelState.AddModelError("", "Try Again.");
                            return View();
                        }
                        return RedirectToAction("Index", "Home", new { user = Session["username"] });
                        
                    }
                    return View();
                
            }
        }

        public Customers GetUser(string _usrName)// tum musterı bılgılerının cekmek ıcın fonksıyon
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                var rcust = (from c in data.Customers
                            where c.UserName == _usrName
                            select c).FirstOrDefault();
                return rcust;
            }
        }      

        public ActionResult Detail(string user)//musterı bılgılerını gormek ıcıcn foınksıyon musterı bılgılerını model olarak htmlle gonderır
            //aldıgı user degerı musterınının sessıonu yada usernameıdir bu usernamei GeyUser fonksıyonu ıcınde kullanın user ıdsını alırız o ıdnın bılgılerının gosterır
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                var usr = data.Customers.Find(GetUser(user).CustomerID);             
                return View(usr);
            }
        }
        public ActionResult Update(string user)//ustekının aynısı sadece html alanları farklı bunda degısıklık yapıp  altdakı fonksıyon ıle gonderıyoruz update yapıyopruz
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                var usr = data.Customers.Find(GetUser(user).CustomerID);             
                return View(usr);
            }
        }

        //musteri update   ust fonsıyonuda gonderılen model bıigilerinde degişiklik yapıp  save e bastıgımız bılgılerı bu fonksıyonu gonerırıyor ve  zaten var olan 
        //satırı  data.Entry(cust).State = EntityState.Modified ile gunleiyoruz daha sonra kayedeyoruz.  ve detail sayfasına yonlendırıyor.
        [HttpPost]
        public ActionResult Update(Customers cust)
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                if (ModelState.IsValid)
                {
                    data.Entry(cust).State = EntityState.Modified;
                    data.SaveChanges();
                    Session["username"] = cust.UserName;
                    return RedirectToAction("Detail", "Accounts",new {user= Session["username"] });
                }
                return View(cust);
            }
        }
    }
}