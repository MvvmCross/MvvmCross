using System;
using MvvmCross.Platform.Core;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views.EventSource
{
    public class MvxEventSourceContentPage : ContentPage, IMvxEventSourcePage
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
