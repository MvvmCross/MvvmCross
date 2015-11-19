// MvxChildViewModelCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxChildViewModelCache : IMvxChildViewModelCache
    {
        private static int _unique = 1;

        private readonly Dictionary<int, IMvxViewModel> _viewModels = new Dictionary<int, IMvxViewModel>();

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
    }
}