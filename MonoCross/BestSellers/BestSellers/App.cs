using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Navigation;
using BestSellers.Controllers;

namespace BestSellers
{
    public class App : MXApplication
    {
        public override void OnAppLoad()
        {
            // Set the application title
            Title = "Best Sellers";

            // Add navigation mappings
            NavigationMap.Add("", new CategoryListController());
            NavigationMap.Add("{Category}", new BookListController());
            NavigationMap.Add("{Category}/{Book}", new BookController());

            // Set default navigation URI
            NavigateOnLoad = "";
        }
    }
}
