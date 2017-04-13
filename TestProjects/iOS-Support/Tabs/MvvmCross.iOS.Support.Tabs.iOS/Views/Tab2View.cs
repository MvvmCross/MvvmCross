using System;
using Cirrious.FluentLayouts.Touch;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Support.Presenters;
using MvvmCross.iOS.Support.Tabs.Core.ViewModels;
using MvvmCross.iOS.Views;
using UIKit;

namespace MvvmCross.iOS.Support.Tabs.iOS.Views
{
    [MvxTabPresentation(MvxTabPresentationMode.Tab, true)]
    public class Tab2View : MvxViewController<Tab2ViewModel>
    {
        private UIButton _btnLogout;

        public Tab2View()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _btnLogout = new UIButton();
            _btnLogout.SetTitle("Logout", UIControlState.Normal);
            _btnLogout.SetTitleColor(UIColor.DarkGray, UIControlState.Normal);

            View.AddSubviews(_btnLogout);
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(
                _btnLogout.WithSameCenterX(View),
                _btnLogout.WithSameCenterY(View)
            );

            var set = this.CreateBindingSet<Tab2View, Tab2ViewModel>();
            set.Bind(_btnLogout).To(vm => vm.LogoutCommand);
            set.Apply();
        }
    }
}

