using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Navigation;
using MonoCross.Console;

using BestSellers;

namespace BestSellers.Console.Views
{
    class BookListView : MXConsoleView<BookList>
    {
        public override void Render()
        {
            System.Console.WriteLine();
            System.Console.WriteLine(Model.CategoryDisplayName);
            System.Console.WriteLine();

            Dictionary<string, string> inputValues = new Dictionary<string, string>();
            foreach (Book book in Model)
            {
                string inputValue = (inputValues.Count + 1).ToString();
                System.Console.WriteLine(inputValue + ". " + book.Title);
                inputValues.Add(inputValue, string.Format("{0}/{1}", book.CategoryEncoded, book.ISBN10));
            }

            System.Console.WriteLine();
            System.Console.WriteLine("Enter Index Value or Blank to go Back");

            DefaultInputAndNavigate(inputValues);
        }
    }
}
