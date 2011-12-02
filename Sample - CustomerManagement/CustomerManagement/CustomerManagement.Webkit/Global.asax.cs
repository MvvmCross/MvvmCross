using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using MonoCross.Navigation;
using MonoCross.Webkit;

using CustomerManagement.Shared.Model;

namespace CustomerManagement.Webkit
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Ignore("favicon.ico");
            routes.Ignore("WebApp/{*mapUri}");
            routes.MapRoute("", "{*mapUri}", new { controller = "App", action = "Render" });
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start()
        {
            // initialize app
            MXWebkitContainer.Initialize(new CustomerManagement.App());
            // add views to container
            MXWebkitContainer.AddView<List<Customer>>(new Views.CustomerListView(), ViewPerspective.Default);
            MXWebkitContainer.AddView<Customer>(new Views.CustomerEditView(), ViewPerspective.Update);
            MXWebkitContainer.AddView<Customer>(new Views.CustomerView(), ViewPerspective.Default);
        }
    }
}