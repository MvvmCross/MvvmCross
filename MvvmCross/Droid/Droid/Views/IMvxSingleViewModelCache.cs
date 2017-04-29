// IMvxSingleViewModelCache.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.OS;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Droid.Views
{
    public interface IMvxSingleViewModelCache
    {
        void Cache(IMvxViewModel toCache, Bundle bundle);

        IMvxViewModel GetAndClear(Bundle bundle);
    }
}