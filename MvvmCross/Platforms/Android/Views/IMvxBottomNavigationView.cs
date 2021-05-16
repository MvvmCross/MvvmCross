using System;
using Android.Views;

namespace MvvmCross.Platforms.Android.Views
{
    public interface IMvxBottomNavigationView
    {
        public AndroidX.ViewPager.Widget.ViewPager ViewPager { get; set; }
        public bool DidRegisterViewModelType(Type viewModelType);
        public void RegisterViewModel(IMenuItem menuItem, Type viewModelType);
    }
}
