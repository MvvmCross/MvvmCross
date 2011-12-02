using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using MonoCross.Navigation;
using MonoCross.Console;

using BestSellers;

namespace BestSellers.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // initialize container
            MXConsoleContainer.Initialize(new BestSellers.App());

            // initialize views
            MXConsoleContainer.AddView<CategoryList>(new Views.CategoryListView(), ViewPerspective.Read);
            MXConsoleContainer.AddView<BookList>(new Views.BookListView(), ViewPerspective.Read);
            MXConsoleContainer.AddView<Book>(new Views.BookView(), ViewPerspective.Read);

            // navigate to first view
            MXConsoleContainer.Navigate(MXContainer.Instance.App.NavigateOnLoad);
        }
    }    
}
