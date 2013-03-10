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
using BestSellers.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Controls;

using BestSellers;

namespace BestSellers.WindowsPhone.Views
{
    public partial class CategoryListView : MvxPhonePage
    {
        public new CategoryListViewModel ViewModel
        {
            get { return (CategoryListViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public CategoryListView()
        {
            InitializeComponent();
        }
    }
}