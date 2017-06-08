// MvxChildViewModelCache.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Core.Views
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

        public bool Exists(Type viewModelType)
        {
            return _viewModels.Values.Any(x => x.GetType() == viewModelType);
        }

        public IMvxViewModel Get(int index)
        {
            IMvxViewModel viewModel;
            _viewModels.TryGetValue(index, out viewModel);
            return viewModel;
        }

        public IMvxViewModel Get(Type viewModelType)
        {
            return _viewModels.Values.FirstOrDefault(x => x.GetType() == viewModelType);
        }

        public void Remove(int index)
        {
            _viewModels.Remove(index);
        }

        public void Remove(Type viewModelType)
        {
            _viewModels.Remove(_viewModels.FirstOrDefault(x => x.Value.GetType() == viewModelType).Key);
        }
    }
}
