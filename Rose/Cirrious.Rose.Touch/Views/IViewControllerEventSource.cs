using System;
using Cirrious.MvvmCross.Droid.Views;

namespace Cirrious.CrossCore.Touch.Views
{
    public interface IViewControllerEventSource : IDisposeSource
    {
        event EventHandler ViewDidLoadCalled;
        event EventHandler<TypedEventArgs<bool>> ViewWillAppearCalled;
        event EventHandler<TypedEventArgs<bool>> ViewDidAppearCalled;
        event EventHandler<TypedEventArgs<bool>> ViewDidDisappearCalled;
        event EventHandler<TypedEventArgs<bool>> ViewWillDisappearCalled;
    }
}