// MvxChildViewModelCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Views
{
    using System.Collections.Generic;

    using MvvmCross.Core.ViewModels;

    public class MvxChildViewModelCache : IMvxChildViewModelCache
    {
        private static int _unique = 1;

        private readonly Dictionary<int, IMvxViewModel> _viewModels = new Dictionary<int, IMvxViewModel>();

        public int Cache(IMvxViewModel viewModel)
        {
            var index = _unique++;
            this._viewModels[index] = viewModel;
            return index;
        }

        public IMvxViewModel Get(int index)
        {
            return this._viewModels[index];
        }

        public void Remove(int index)
        {
            this._viewModels.Remove(index);
        }
    }
}