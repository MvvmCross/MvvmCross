using System;
using MvvmCross.Platform.Core;

namespace MvvmCross.Platform.Mac.Views
{
    public interface IMvxEventSourceWindowController : IMvxDisposeSource
    {
        event EventHandler ViewDidLoadCalled;
    }
}
