// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Android.Views
{
    public class MvxSingleViewModelCache
        : IMvxSingleViewModelCache
    {
        private const string BundleCacheKey = "__mvxVMCacheKey";

        private int _counter;

        private WeakReference<IMvxViewModel>? _currentViewModel;

        public void Cache(IMvxViewModel toCache, Bundle bundle)
        {
            _currentViewModel = new WeakReference<IMvxViewModel>(toCache);
            _counter++;

            if (_currentViewModel == null)
            {
                return;
            }

            bundle.PutInt(BundleCacheKey, _counter);
        }

        public IMvxViewModel? GetAndClear(Bundle? bundle)
        {
            try
            {
                if (bundle == null)
                    return null;

                if (_currentViewModel?.TryGetTarget(out var storedViewModel) == true)
                {
                    var key = bundle.GetInt(BundleCacheKey);
                    var toReturn = key == _counter ? storedViewModel : null;
                    return toReturn;
                }
            }
            finally
            {
                _currentViewModel = null;
            }

            return null;
        }
    }
}
