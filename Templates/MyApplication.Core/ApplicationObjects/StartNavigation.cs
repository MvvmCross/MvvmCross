using Cirrious.MvvmCross.ViewModels;
using MyApplication.Core.ViewModels;

namespace MyApplication.Core.ApplicationObjects
{
    public class StartNavigation
        : MvxNavigatingObject
        , IMvxStartNavigation
    {
        public void Start()
        {
            RequestNavigate<HomeViewModel>();
        }

        public bool ApplicationCanOpenBookmarks
        {
            get { return true; }
        }
    }
}
