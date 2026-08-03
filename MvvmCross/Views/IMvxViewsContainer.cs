// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;
using MvvmCross.ViewModels;

namespace MvvmCross.Views
{
    public interface IMvxViewsContainer : IMvxViewFinder
    {
        void AddAll(IDictionary<Type, Type> viewModelViewLookup);

        void Add(Type viewModelType, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)] Type viewType);

        void Add<TViewModel, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)] TView>()
            where TViewModel : IMvxViewModel
            where TView : IMvxView;

        void AddSecondary(IMvxViewFinder finder);

        void SetLastResort(IMvxViewFinder finder);
    }
}
