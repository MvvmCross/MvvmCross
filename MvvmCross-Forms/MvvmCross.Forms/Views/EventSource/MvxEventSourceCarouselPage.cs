using System;
using MvvmCross.Forms.Views.EventSource;
using MvvmCross.Platform.Core;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views.EventSource
{
    public class MvxEventSourceCarouselPage : CarouselPage, IMvxEventSourcePage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
            AppearingCalled.Raise(this);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            DisappearingCalled.Raise(this);
        }

        public event EventHandler AppearingCalled;
        public event EventHandler DisappearingCalled;
    }
}
