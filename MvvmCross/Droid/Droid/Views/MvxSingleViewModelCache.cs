// MvxSingleViewModelCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.OS;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxSingleViewModelCache
        : IMvxSingleViewModelCache
    {
        private const string BundleCacheKey = "__mvxVMCacheKey";

        private int _counter;

        private IMvxViewModel _currentViewModel;

        public void Cache(IMvxViewModel toCache, Bundle bundle)
        {
            _currentViewModel = toCache;
            _counter++;

            if (_currentViewModel == null)
            {
                return;
            }

            bundle.PutInt(BundleCacheKey, _counter);
        }

        public IMvxViewModel GetAndClear(Bundle bundle)
        {
            var storedViewModel = _currentViewModel;
            _currentViewModel = null;

            if (bundle == null)
                return null;

            var key = bundle.GetInt(BundleCacheKey);
            var toReturn = (key == _counter) ? storedViewModel : null;
            return toReturn;
        }
    }
}