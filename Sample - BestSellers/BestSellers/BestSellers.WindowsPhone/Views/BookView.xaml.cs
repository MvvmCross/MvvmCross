using System;
using BestSellers.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Views;
using System.Windows.Media.Imaging;

namespace BestSellers.WindowsPhone.Views
{
    public partial class BookView : MvxPhonePage
    {
        public new BookViewModel ViewModel
        {
            get { return (BookViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public BookView()
        {
            InitializeComponent();
        }

            // AmazonImage.Source = new BitmapImage(new Uri(Model.AmazonImageUrl, UriKind.Absolute));
    }
}