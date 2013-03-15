using BestSellers.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Views;

namespace BestSellers.WindowsPhone.Views
{
    public partial class BookListView : MvxPhonePage
    {
        public new BookListViewModel ViewModel
        {
            get { return (BookListViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public BookListView()
        {
            InitializeComponent();
        }
    }    
}