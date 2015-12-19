// IMvxIntentResultSink.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Droid.Platform
{
    using MvvmCross.Platform.Droid.Views;

    public interface IMvxIntentResultSink
    {
        void OnResult(MvxIntentResultEventArgs result);
    }
}