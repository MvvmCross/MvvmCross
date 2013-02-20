using System;
using Cirrious.CrossCore.Interfaces.Core;

namespace Cirrious.CrossCore.Touch.Views
{
    public interface IMvxEventSourceViewController : IMvxDisposeSource
    {
        event EventHandler ViewDidLoadCalled;
        event EventHandler<MvxTypedEventArgs<bool>> ViewWillAppearCalled;
        event EventHandler<MvxTypedEventArgs<bool>> ViewDidAppearCalled;
        event EventHandler<MvxTypedEventArgs<bool>> ViewDidDisappearCalled;
        event EventHandler<MvxTypedEventArgs<bool>> ViewWillDisappearCalled;
    }
}