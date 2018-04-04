using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.ApplicationInsights;


namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private TelemetryClient telemetry = new TelemetryClient();

        public ActionResult Index()
        {
            using (bookStoreDBEntities data = new bookStoreDBEntities())
            {
                ViewBag.cata = data.Categories.ToList().OrderBy(x => x.Name);
               
                ViewBag.book =data.Products.Where(b=>b.active == true).OrderByDescending(u => u.ProductID).Take(8).ToList();
                ViewBag.pro = data.Promo.ToList();

                ViewBag.rec2 = data.Products.Where(b => b.active == true).OrderByDescending(b => b.SupplierID).Take(4).ToList();
                ViewBag.rec = data.Products.Where(b => b.active == true).OrderByDescending(b => b.Price).Take(4).ToList();

               
                telemetry.TrackPageView("HomePage");
                return View();
            }
              
                
        }
       

        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendMail( string email, string subject, string message)
        {
            
                telemetry.TrackPageView("ContactPage");
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "ozankck23234@gmail.com";
                WebMail.Password = "as2457er";
                WebMail.SmtpPort = 587;
                WebMail.Send(
                        "ozankck@msn.com",                
                        subject,
                        message,
                        email
                    );

                return RedirectToAction("Contact");

            
        }
        public string Gonderildi()
        {
            return "Your message sended";
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
    }
}