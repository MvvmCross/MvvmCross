// IMvxViewModelByNameRegistry.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;

namespace Cirrious.MvvmCross.ViewModels
{
    public interface IMvxViewModelByNameRegistry
    {
        void Add(Type viewModelType);

        void Add<TViewModel>() where TViewModel : IMvxViewModel;

        void AddAll(Assembly assembly);
    }
}