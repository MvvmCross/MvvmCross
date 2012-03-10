using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Navigation;
using MonoCross.Console;

using BestSellers;

namespace BestSellers.Console.Views
{
    class BookView : MXConsoleView<Book>
    {
        public override void Render()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Book Details");
            System.Console.WriteLine();

            System.Console.WriteLine(Model.Title);
            System.Console.WriteLine(Model.Contributor);
            System.Console.WriteLine(string.Format("${0}", Model.Price));
            System.Console.WriteLine();
            System.Console.WriteLine(Model.Description);
            System.Console.WriteLine();
            System.Console.WriteLine("Enter to Continue");

            DefaultInputAndNavigate(null);
        }
    }
}
