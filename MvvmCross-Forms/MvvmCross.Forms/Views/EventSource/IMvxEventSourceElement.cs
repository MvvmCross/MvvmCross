using System;
using MvvmCross.Platform.Core;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views.EventSource
{
    public interface IMvxEventSourceElement
    {
        event EventHandler BindingContextChangedCalled;

        event EventHandler ParentSetCalled;
    }
}