using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.WindowsPhone;
using Cirrious.MvvmCross.WindowsPhone.Views;
using CustomerManagement.ViewModels;
using Microsoft.Phone.Controls;
using CustomerManagement.Shared.Model;
using Microsoft.Phone.Shell;

namespace CustomerManagement.WindowsPhone
{
    public class BaseCustomerEditView : MvxPhonePage<EditCustomerViewModel> { }

    [MvxPhoneView("/Views/CustomerEditView.xaml")]
    public partial class CustomerEditView : BaseCustomerEditView
    {
        public CustomerEditView()
        {
            InitializeComponent();
        }

        public override void Render()
        {
            this.DataContext = Model;
        }

        /*
        private void InitAppBar()
        {
            ApplicationBar appBar = new ApplicationBar();

            var addButton = new ApplicationBarIconButton(new Uri("images/appbar.save.rest.png", UriKind.Relative));
            addButton.Click += new EventHandler(saveButton_Click);
            addButton.Text = "Save";
            appBar.Buttons.Add(addButton);

            ApplicationBar = appBar;
        }

        void saveButton_Click(object sender, EventArgs e)
        {
            if (Model.ID == "0")
                this.Navigate<CustomerController>("Create");
            else
                this.Navigate<CustomerController>("Update");
        }

        void backButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
        */
    }
}