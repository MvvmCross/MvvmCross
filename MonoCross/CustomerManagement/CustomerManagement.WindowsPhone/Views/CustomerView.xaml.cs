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
using Microsoft.Phone.Controls;

using MonoCross.Navigation;
using MonoCross.WindowsPhone;

using CustomerManagement.Shared.Model;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;

namespace CustomerManagement.WindowsPhone
{
    public class BaseCustomerView : MXPhonePage<Customer> { }

    [MXPhoneView("/Views/CustomerView.xaml")]
    public partial class CustomerView : BaseCustomerView
    {
        public CustomerView()
        {
            InitializeComponent();

            ApplicationTitle.Text = MXContainer.Instance.App.Title;

            // events for 
            this.textAddress.Tap += new EventHandler<GestureEventArgs>(textAddress_Tap);
            this.textPhone.Tap += new EventHandler<GestureEventArgs>(textPhone_Tap);
            this.textWebsite.Tap += new EventHandler<GestureEventArgs>(textWebsite_Tap);

            InitAppBar();
        }

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

        void editButton_Click(object sender, EventArgs e)
        {
            this.Navigate(string.Format("Customers/{0}/EDIT", Model.ID));
        }

        void deleteButton_Click(object sender, EventArgs e)
        {
            this.Navigate(string.Format("Customers/{0}/DELETE", Model.ID));
        }

        void backButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        public override void Render()
        {
            this.DataContext = Model;
        }

        void textWebsite_Tap(object sender, GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri(Model.Website);
            webBrowserTask.Show();
        }

        void textPhone_Tap(object sender, GestureEventArgs e)
        {
            PhoneCallTask pct = new PhoneCallTask();
            pct.DisplayName = Model.Name;
            pct.PhoneNumber = Model.PrimaryPhone;
            pct.Show();  
        }

        void textAddress_Tap(object sender, GestureEventArgs e)
        {
            string googleAddress = string.Format("{0} {1}\n{2}, {3}  {4}",
                        Model.PrimaryAddress.Street1, Model.PrimaryAddress.Street2,
                        Model.PrimaryAddress.City, Model.PrimaryAddress.State, Model.PrimaryAddress.Zip);
            googleAddress = Uri.EscapeUriString(googleAddress);

            string url = string.Format("http://maps.google.com/maps?q={0}", googleAddress);

            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri(url);
            webBrowserTask.Show();
        }
    }
}