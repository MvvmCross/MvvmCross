// IMvxViewModelByNameRegistry.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using System;
    using System.Reflection;

    public interface IMvxViewModelByNameRegistry
    {
        void Add(Type viewModelType);

        void Add<TViewModel>() where TViewModel : IMvxViewModel;

        void AddAll(Assembly assembly);
    }
}