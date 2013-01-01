// MvxAndroidSubViewModelCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxAndroidSubViewModelCache : IMvxAndroidSubViewModelCache
    {
        private static int _unique = 1;

        private readonly Dictionary<int, IMvxViewModel> _viewModels = new Dictionary<int, IMvxViewModel>();

        #region IMvxAndroidSubViewModelCache Members

        public int Cache(IMvxViewModel viewModel)
        {
            var index = _unique++;
            _viewModels[index] = viewModel;
            return index;
        }

        public IMvxViewModel Get(int index)
        {
            return _viewModels[index];
        }

        public void Remove(int index)
        {
            _viewModels.Remove(index);
        }

        #endregion
    }
}