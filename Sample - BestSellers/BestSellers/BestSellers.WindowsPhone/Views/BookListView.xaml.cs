using BestSellers.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Views;

namespace BestSellers.WindowsPhone.Views
{
    public class BaseBookListView : MvxPhonePage<BookListViewModel> { }

    public partial class BookListView : BaseBookListView
    {
        public BookListView()
        {
            InitializeComponent();
        }
    }    
}