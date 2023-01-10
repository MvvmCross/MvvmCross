// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Presenters
{
#nullable enable
    public interface IMvxAttributeViewPresenter : IMvxViewPresenter
    {
        IMvxViewModelTypeFinder? ViewModelTypeFinder { get; }
        IMvxViewsContainer? ViewsContainer { get; }
        IDictionary<Type, MvxPresentationAttributeAction>? AttributeTypesToActionsDictionary { get; }
        void RegisterAttributeTypes();

        [RequiresUnreferencedCode("GetPresentationAttribute is not trimming compatible")]
        MvxBasePresentationAttribute GetPresentationAttribute(MvxViewModelRequest request);
        MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType);
        MvxBasePresentationAttribute? GetOverridePresentationAttribute(MvxViewModelRequest request, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)] Type viewType);
    }
#nullable restore
}
