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
    }
}