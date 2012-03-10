using System;
using BestSellers.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Views;
using System.Windows.Media.Imaging;

namespace BestSellers.WindowsPhone.Views
{
    public class BaseBookView : MvxPhonePage<BookViewModel> { }

    public partial class BookView : BaseBookView
    {
        public BookView()
        {
            InitializeComponent();
        }

            // AmazonImage.Source = new BitmapImage(new Uri(Model.AmazonImageUrl, UriKind.Absolute));
    }
}