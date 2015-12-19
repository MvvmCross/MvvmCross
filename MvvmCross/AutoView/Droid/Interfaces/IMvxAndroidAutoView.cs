// IMvxAndroidAutoView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Droid.Interfaces
{
    using MvvmCross.AutoView.Interfaces;
    using MvvmCross.Droid.Views;

    public interface IMvxAndroidAutoView
        : IMvxAndroidView
          , IMvxAutoView
    {
    }
}