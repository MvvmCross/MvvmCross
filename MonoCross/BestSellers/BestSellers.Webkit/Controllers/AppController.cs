using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MonoCross.Navigation;
using MonoCross.Webkit;

using BestSellers;

namespace BestSellers.Webkit.Controllers
{
    [HandleError]
    public class AppController : Controller
    {
        public ActionResult Render(string mapUri)
        {
            String url = (mapUri == null) ? MXContainer.Instance.App.NavigateOnLoad : mapUri;
            MXWebkitContainer.Navigate(url, this.Request);
            return null;
        }
    }
}
