using System;
using System.Linq;
using Cirrious.MvvmCross.Console.Views;
using TwitterSearch.Core.ViewModels;

namespace TwitterSearch.UI.Console.Views
{
    public class TwitterView : MvxConsoleView<TwitterViewModel>
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

            if (ViewModel.IsSearching)
            {
                System.Console.WriteLine("Searching");
                return;
            }

            System.Console.WriteLine("Tweets");
            System.Console.WriteLine();

            if (ViewModel.Tweets == null)
            {
                System.Console.WriteLine("NULL!");
            }
            else if (ViewModel.Tweets.Any())
            {
                int index = 1;
                foreach (var tweet in ViewModel.Tweets)
                {
                    System.Console.WriteLine(index.ToString() + ". " + tweet.Title);
                    System.Console.WriteLine();
                    index++;
                }
            }
            else
            {
                System.Console.WriteLine("NULL!");
            }

            System.Console.WriteLine();
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine("Enter (B)ACK to go Back");
            System.Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
                                                                                    