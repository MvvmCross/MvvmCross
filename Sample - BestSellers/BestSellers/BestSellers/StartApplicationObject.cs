using BestSellers.ViewModels;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;

namespace BestSellers
{
    public class StartApplicationObject
        : MvxApplicationObject
          , IMvxStartNavigation
    {
        public void Start()
        {
            RequestNavigate<CategoryListViewModel>();
        }

        public bool ApplicationCanOpenBookmarks
        {
            get { return false; }
        }
    }
}