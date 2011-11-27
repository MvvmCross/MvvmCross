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
using CustomerManagement.Controllers;
using Microsoft.Phone.Controls;

using MonoCross.Navigation;
using MonoCross.WindowsPhone;

using CustomerManagement.Shared.Model;
using Microsoft.Phone.Shell;

using Cirrious.MonoCross.Extensions.ExtensionMethods;

namespace CustomerManagement.WindowsPhone
{
    public class BaseCustomerListView : MXPhonePage<List<Customer>> { }

    [MXPhoneView("/Views/CustomerListView.xaml")]
    public partial class CustomerListView : BaseCustomerListView
    {
        // Constructor
        public CustomerListView()
        {
            InitializeComponent();

            ApplicationTitle.Text = MXContainer.Instance.App.Title;
            PageTitle.Text = "Customers";

            InitAppBar();
        }

        private void InitAppBar()
        {
            ApplicationBar appBar = new ApplicationBar();

            var addButton = new ApplicationBarIconButton(new Uri("images/appbar.add.rest.png", UriKind.Relative));
            addButton.Click += new EventHandler(addButton_Click);
            addButton.Text = "Add";
            appBar.Buttons.Add(addButton);

            ApplicationBar = appBar;
        }

        void addButton_Click(object sender, EventArgs e)
        {
            this.Navigate<CustomerController>("New");
        }

        public override void Render()
        {
            foreach (var customer in Model)
                listBox.Items.Add(customer);

            listBox.SelectionChanged += new SelectionChangedEventHandler(listBox_SelectionChanged);

            // remove the splash screen that was shown just before this
            NavigationService.RemoveBackEntry();
        }

        void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 1)
                return;

            Customer c = e.AddedItems[0] as Customer;

            listBox.SelectedIndex = -1;

            this.Navigate<CustomerController>("Details", new Dictionary<string, string>() {{"customerId",  c.ID}});
        }
    }

}