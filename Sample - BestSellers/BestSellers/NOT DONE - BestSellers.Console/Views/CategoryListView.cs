using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Console;
using MonoCross.Navigation;

using BestSellers;

namespace BestSellers.Console.Views
{
    public class CategoryListView : MXConsoleView<CategoryList>
    {
        public override void Render()
        {
            System.Console.WriteLine("Categories");
            System.Console.WriteLine();

            Dictionary<string, string> inputValues = new Dictionary<string, string>();
            foreach (Category category in Model)
            {
                string inputValue = (inputValues.Count + 1).ToString();
                System.Console.WriteLine(inputValue + ". " + category.DisplayName);
                inputValues.Add(inputValue, category.ListNameEncoded);
            }

            System.Console.WriteLine();
            System.Console.WriteLine("Enter Category Index Value or Blank to go Back");

            DefaultInputAndNavigate(inputValues);
        }
    }
}
