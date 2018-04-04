using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Controllers;

public class ErrorController : Controller
{


    public ActionResult Index()
    {
        ViewBag.Title = "Regular Error";
        return View();
    }

    public ActionResult NotFound()
    {
        

            ViewBag.Title = "Error 404 - File not Found";
       
            return View("Index");
        
    }

}