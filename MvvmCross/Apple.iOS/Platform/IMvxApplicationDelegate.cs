using UIKit;
using MvvmCross.Core.Platform;

namespace MvvmCross.iOS.Platform
{
    public interface IMvxApplicationDelegate
        : IUIApplicationDelegate
        , IMvxLifetime
    {
    }
}

