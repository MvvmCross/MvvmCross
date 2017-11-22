// IMvxSavedStateConverter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.OS;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Droid.Platform
{
    public interface IMvxSavedStateConverter
    {
        IMvxBundle Read(Bundle bundle);

        void Write(Bundle bundle, IMvxBundle savedState);
    }
}