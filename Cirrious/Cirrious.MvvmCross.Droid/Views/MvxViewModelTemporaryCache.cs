// MvxViewModelTemporaryCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.OS;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxViewModelTemporaryCache
        : IMvxViewModelTemporaryCache
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
            var key = bundle.GetInt(BundleCacheKey);
            var toReturn = (key == _counter) ? _currentViewModel : null;
            _currentViewModel = null;
            return toReturn;
        }
    }
}