// IMvxChildViewModelCache.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Core.Views
{
    public interface IMvxChildViewModelCache
    {
        int Cache(IMvxViewModel viewModel);

        IMvxViewModel Get(int index);

        IMvxViewModel Get(Type viewModelType);

        void Remove(int index);

        void Remove(Type viewModelType);

        bool Exists(Type viewModelType);
    }
}
