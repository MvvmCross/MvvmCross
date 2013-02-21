using System;
using Cirrious.CrossCore.Interfaces.Core;

namespace Cirrious.CrossCore.Touch.Views
{
    public interface IMvxEventSourceViewController : IMvxDisposeSource
    {
        event EventHandler ViewDidLoadCalled;
        event EventHandler<MvxValueEventArgs<bool>> ViewWillAppearCalled;
        event EventHandler<MvxValueEventArgs<bool>> ViewDidAppearCalled;
        event EventHandler<MvxValueEventArgs<bool>> ViewDidDisappearCalled;
        event EventHandler<MvxValueEventArgs<bool>> ViewWillDisappearCalled;
    }
}