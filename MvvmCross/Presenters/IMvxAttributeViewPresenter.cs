// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Presenters
{
    public interface IMvxAttributeViewPresenter : IMvxViewPresenter
    {
        IMvxViewModelTypeFinder? ViewModelTypeFinder { get; }
        IMvxViewsContainer? ViewsContainer { get; }
        IDictionary<Type, MvxPresentationAttributeAction>? AttributeTypesToActionsDictionary { get; }
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Getting presentation attribute action uses type hierarchy checks and may call GetPresentationAttribute/CreatePresentationAttribute which require unreferenced code.")]
        void RegisterAttributeTypes();

        //TODO: Maybe move those to helper class
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Getting presentation attribute uses type hierarchy checks and may call CreatePresentationAttribute which requires unreferenced code.")]
        MvxBasePresentationAttribute GetPresentationAttribute(MvxViewModelRequest request);

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Creates presentation attributes based on runtime view types; type hierarchy checks may not be preserved during trimming.")]
        MvxBasePresentationAttribute CreatePresentationAttribute(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? viewModelType,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? viewType);

        MvxBasePresentationAttribute? GetOverridePresentationAttribute(
            MvxViewModelRequest request,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)] Type viewType);
    }
}
