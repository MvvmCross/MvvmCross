// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace MvvmCross.ViewModels
{
#nullable enable
    public class MvxChildViewModelCache : IMvxChildViewModelCache
    {
        private readonly Dictionary<int, IMvxViewModel> _viewModels = new Dictionary<int, IMvxViewModel>();
        private int _unique = 1;

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
            _viewModels.TryGetValue(index, out IMvxViewModel viewModel);
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
#nullable restore
}
