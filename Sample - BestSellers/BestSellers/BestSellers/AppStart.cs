using BestSellers.ViewModels;
using Cirrious.MvvmCross.ViewModels;

namespace BestSellers
{
    public class AppStart
        : MvxNavigatingObject
          , IMvxAppStart
    {
        public void Start(object hint = null)
        {
            ShowViewModel<CategoryListViewModel>();
        }

        public bool ApplicationCanOpenBookmarks
        {
            get { return false; }
        }
    }
}