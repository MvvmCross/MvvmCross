using Cirrious.MvvmCross.ViewModels;
using MyApplication.Core.ViewModels;

namespace MyApplication.Core.ApplicationObjects
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
