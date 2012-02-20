using Cirrious.MvvmCross.Console.Views;
using CustomerManagement.Core.ViewModels;

namespace CustomerManagement.Console.Views
{
    class BaseCustomerEditView<TViewModel> 
        : MvxConsoleView<TViewModel> 
        where TViewModel : BaseEditCustomerViewModel
    {
        protected override void  OnViewModelChanged()
        {
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            System.Console.Clear();
            System.Console.WriteLine("Customer Details");
            System.Console.WriteLine();
            System.Console.WriteLine("Name: " + ViewModel.Customer.Name);
            System.Console.WriteLine("Website: " + ViewModel.Customer.Website);
            System.Console.WriteLine("Primary Phone: " + ViewModel.Customer.PrimaryPhone);

            System.Console.WriteLine();
            System.Console.WriteLine("To change Name, use (N)AME <new name>");
            System.Console.WriteLine("To change Website, use (W)WW <new url>");
            System.Console.WriteLine("To change PHONE, use (P)HONE <new phone>");
            System.Console.WriteLine("(B)ACK to Cancel - or (S)AVE to Save");
        }

        public override bool HandleInput(string input)
        {
            var capsInput = input.Trim().ToUpper();
            switch (capsInput)
            {
                case "SAVE":
                case "S":
                    ViewModel.SaveCommand.Execute();
                    return true;
            }

            if (capsInput.StartsWith("N ") || capsInput.StartsWith("NAME "))
            {
                ViewModel.Customer.Name = GetParameterText(input);
                RefreshDisplay();
                return true;
            }

            if (capsInput.StartsWith("W ") || capsInput.StartsWith("WWW "))
            {
                ViewModel.Customer.Website = GetParameterText(input);
                RefreshDisplay();
                return true;
            }

            if (capsInput.StartsWith("P ") || capsInput.StartsWith("PHONE "))
            {
                ViewModel.Customer.PrimaryPhone = GetParameterText(input);
                RefreshDisplay();
                return true;
            }
            return base.HandleInput(input);
        }

        private string GetParameterText(string input)
        {
            var split = input.Split(new [] {' '}, 2);

            if (split.Length < 2)
                return "Empty!";

            var candidate = split[1].Trim();
            if (string.IsNullOrEmpty(candidate))
                return "Empty!!";

            return candidate;
        }
    }
}