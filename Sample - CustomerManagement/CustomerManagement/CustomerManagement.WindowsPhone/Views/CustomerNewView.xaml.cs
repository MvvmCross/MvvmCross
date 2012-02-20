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
using CustomerManagement.Core.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace CustomerManagement.WindowsPhone
{
    public class BaseCustomerNewView : MvxPhonePage<NewCustomerViewModel> { }

    [MvxPhoneView("/Views/CustomerNewView.xaml")]
    public partial class CustomerNewView : BaseCustomerNewView
    {
        public CustomerNewView()
        {
            InitializeComponent();
        }
    }
}