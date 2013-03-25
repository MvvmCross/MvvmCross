using System;
using Cirrious.MvvmCross.Console.Views;
using TwitterSearch.Core.ViewModels;

namespace TwitterSearch.UI.Console.Views
{
    class HomeView : MvxConsoleView<HomeViewModel>
    {
        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();
            ViewModel.PropertyChanged += (sender, args) => RefreshDisplay();
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            System.Console.BackgroundColor = ConsoleColor.Black;
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.Clear();
            System.Console.WriteLine("Current Search");
            System.Console.WriteLine();

            System.Console.WriteLine(ViewModel.SearchText);
            System.Console.WriteLine();
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine("To search, (S)EARCH");
            System.Console.WriteLine("To get a random term, (R)ANDOM");
            System.Console.WriteLine("To enter a new search term, (E)DIT {text}");
            System.Console.ForegroundColor = ConsoleColor.White;
        }

        public override bool HandleInput(string input)
        {
            input = input.Trim().ToUpper();
            switch (input)
            {
                case "SEARCH":
                case "S":
                    ViewModel.Commands["Search"].Execute(null);
                    return true;
                case "RANDOM":
                case "R":
                    ViewModel.Commands["PickRandom"].Execute(null);
                    return true;
                default:
                    string searchTerm = null;
                    if (input.StartsWith("EDIT ") && input.Length > 5)
                        searchTerm = input.Substring(5);
                    else if (input.StartsWith("E ") && input.Length > 2)
                        searchTerm = input.Substring(2);
                    if (searchTerm != null)
                    {
                        ViewModel.SearchText = searchTerm;
                        return true;
                    }
                    break;
            }

            return base.HandleInput(input);
        }
    }
}
