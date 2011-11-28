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
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces;
using Cirrious.MvvmCross.Interfaces.Application;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.WindowsPhone;
using Cirrious.MvvmCross.WindowsPhone.Views;
using CustomerManagement.ViewModels;
using Microsoft.Phone.Controls;
using CustomerManagement.Shared.Model;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;

namespace CustomerManagement.WindowsPhone
{
    public class BaseCustomerView : MvxPhonePage<DetailsCustomerViewModel> { }

    [MvxPhoneView("/Views/CustomerView.xaml")]
    public partial class CustomerView : BaseCustomerView, IMvxServiceConsumer<IMvxApplicationTitle>
    {
        public CustomerView()
        {
            InitializeComponent();

            ApplicationTitle.Text = this.GetService<IMvxApplicationTitle>().Title;
        }

        /*
        private void InitAppBar()
        {
            ApplicationBar appBar = new ApplicationBar();

            var backButton = new ApplicationBarIconButton(new Uri("images/appbar.back.rest.png", UriKind.Relative));
            backButton.Click += new EventHandler(backButton_Click);
            backButton.Text = "Back";
            appBar.Buttons.Add(backButton);

            var editButton = new ApplicationBarIconButton(new Uri("images/appbar.edit.rest.png", UriKind.Relative));
            editButton.Click += new EventHandler(editButton_Click);
            editButton.Text = "Edit";
            appBar.Buttons.Add(editButton);

            var deleteButton = new ApplicationBarIconButton(new Uri("images/appbar.delete.rest.png", UriKind.Relative));
            deleteButton.Click += new EventHandler(deleteButton_Click);
            deleteButton.Text = "Delete";
            appBar.Buttons.Add(deleteButton);

            ApplicationBar = appBar;
        }
        */
        public class Test
        {
            public string customerId { get; set; }
        }

        /*
        void editButton_Click(object sender, EventArgs e)
        {
            this.Navigate<CustomerController>("Edit", new Test { customerId = Model.Customer.ID });
        }

        void deleteButton_Click(object sender, EventArgs e)
        {
            this.Navigate<CustomerController>("Delete", new { customerId = Model.Customer.ID });
        }

        void backButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
        */

        public override void Render()
        {
            // nothing to do!
            base.Render();
        }

    }
}