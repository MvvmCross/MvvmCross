using Cirrious.MvvmCross.ViewModels;
using TwitterSearch.Core.ViewModels;

namespace TwitterSearch.Core
{
    public class AppStart
        : MvxNavigatingObject
        , IMvxAppStart
    {
        public void Start(object hint = null)
        {
            RequestNavigate<HomeViewModel>();
        }

        public bool ApplicationCanOpenBookmarks
        {
            get { return true; }
        }
    }
}
