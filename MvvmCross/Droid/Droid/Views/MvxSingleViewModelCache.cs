// MvxSingleViewModelCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Views
{
    using Android.OS;

    using MvvmCross.Core.ViewModels;

    public class MvxSingleViewModelCache
        : IMvxSingleViewModelCache
    {
        private const string BundleCacheKey = "__mvxVMCacheKey";

        private int _counter;

        private IMvxViewModel _currentViewModel;

        public void Cache(IMvxViewModel toCache, Bundle bundle)
        {
            this._currentViewModel = toCache;
            this._counter++;

            if (this._currentViewModel == null)
            {
                return;
            }

            bundle.PutInt(BundleCacheKey, this._counter);
        }

        public IMvxViewModel GetAndClear(Bundle bundle)
        {
            var storedViewModel = this._currentViewModel;
            this._currentViewModel = null;

            if (bundle == null)
                return null;

            var key = bundle.GetInt(BundleCacheKey);
            var toReturn = (key == this._counter) ? storedViewModel : null;
            return toReturn;
        }
    }
}