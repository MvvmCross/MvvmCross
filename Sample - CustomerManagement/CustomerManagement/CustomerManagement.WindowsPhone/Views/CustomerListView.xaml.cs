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
using Microsoft.Phone.Shell;
using Phone7.Fx.Controls;

namespace CustomerManagement.WindowsPhone
{
    public class BaseCustomerListView : MvxPhonePage<CustomerListViewModel> { }

    [MvxPhoneView("/Views/CustomerListView.xaml")]
    public partial class CustomerListView : BaseCustomerListView, IMvxServiceConsumer<IMvxApplicationTitle>
    {
        // Constructor
        public CustomerListView()
        {
            InitializeComponent();

            ApplicationTitle.Text = this.GetService<IMvxApplicationTitle>().Title;
            PageTitle.Text = "Customers";
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

#warning This is a nasty hack to force updating - just because our model doesn't do INotifyPropertyChanged very well
            DataContext = null;
            DataContext = ViewModel;
        }
    }
}