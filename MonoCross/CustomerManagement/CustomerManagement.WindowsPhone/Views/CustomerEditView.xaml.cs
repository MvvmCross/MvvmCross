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
using Microsoft.Phone.Shell;

namespace CustomerManagement.WindowsPhone
{
    public class BaseCustomerEditView : MXPhonePage<Customer> { }

    [MXPhoneView("/Views/CustomerEditView.xaml")]
    public partial class CustomerEditView : BaseCustomerEditView
    {
        public CustomerEditView()
        {
            InitializeComponent();

            InitAppBar();
        }

        public override void Render()
        {
            this.DataContext = Model;
        }

        private void InitAppBar()
        {
            ApplicationBar appBar = new ApplicationBar();

            var backButton = new ApplicationBarIconButton(new Uri("images/appbar.back.rest.png", UriKind.Relative));
            backButton.Click += new EventHandler(backButton_Click);
            backButton.Text = "Back";
            appBar.Buttons.Add(backButton);

            var addButton = new ApplicationBarIconButton(new Uri("images/appbar.save.rest.png", UriKind.Relative));
            addButton.Click += new EventHandler(saveButton_Click);
            addButton.Text = "Save";
            appBar.Buttons.Add(addButton);

            ApplicationBar = appBar;
        }

        void saveButton_Click(object sender, EventArgs e)
        {
            this.Navigate("Customers/" + Model.ID + (Model.ID == "0" ? "/CREATE" : "/UPDATE"));
        }

        void backButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}