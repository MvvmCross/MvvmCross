using System;
using MvvmCross.Platform.Core;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views.EventSource
{
    public class MvxEventSourceContentView : ContentView, IMvxEventSourceElement
    {
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContextChangedCalled.Raise(this);
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            ParentSetCalled.Raise(this);
        }

        public event EventHandler BindingContextChangedCalled;
        public event EventHandler ParentSetCalled;
    }
}
