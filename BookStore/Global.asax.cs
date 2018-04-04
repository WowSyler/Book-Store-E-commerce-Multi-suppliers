using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BookStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application.Add("username", 0);

            
        }

        protected void Session_Start(object sender, EventArgs e)// sessiıon sayar  her yenı bir sessionda   toplamı bir arttırır.
        {
            Application.Lock();        
            Application["username"] = (int)Application["username"] + 1;
            Application.UnLock();
        }

        protected void Session_End(object sender, EventArgs e) // her bır sessioın dustugunde bir eksiltir.
        {
            Application.Lock();
            Application["username"] = (int)Application["username"] - 1;
            Application.UnLock();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Application.Remove("username");
        }

        
        protected void Application_Error(object sender, EventArgs e)
        {
            

            //Show the custom error page...
            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";

            if ((Context.Server.GetLastError() is HttpException) && ((Context.Server.GetLastError() as HttpException).GetHttpCode() != 404))
            {
                routeData.Values["action"] = "Index";
            }
            else
            {
                // Handle 404 error and response code
                Response.StatusCode = 404;
                routeData.Values["action"] = "NotFound";
            }
           
            IController errorController = new ErrorController();
            HttpContextWrapper wrapper = new HttpContextWrapper(Context);
            var rc = new System.Web.Routing.RequestContext(wrapper, routeData);
            errorController.Execute(rc);
        }
        
    }
}
