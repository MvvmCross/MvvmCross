using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using MonoCross.Navigation;
using MonoCross.Webkit;

using BestSellers;

namespace BestSellers.Webkit
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = false;
            routes.IgnoreRoute("WebApp/{*pathInfo}");
            routes.Ignore("favicon.ico");
            routes.MapRoute("", "{*mapUri}", new { controller = "App", action = "Render" });
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start()
        {
            // initialize app
            MXWebkitContainer.Initialize(new BestSellers.App());
            // add views to container
            MXWebkitContainer.AddView<CategoryList>(new Views.CategoryListView(), ViewPerspective.Read);
            MXWebkitContainer.AddView<BookList>(new Views.BookListView(), ViewPerspective.Read);
            MXWebkitContainer.AddView<Book>(new Views.BookView(), ViewPerspective.Read);
        }
    }
}