// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.IoC;
using MvvmCross.Logging;
using MvvmCross.Views;

namespace MvvmCross.ViewModels;

public class MvxViewModelViewTypeFinder(
        IMvxViewModelByNameLookup viewModelByNameLookup,
        IMvxNameMapping viewToViewModelNameMapping)
    : IMvxViewModelTypeFinder
{
    [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)]
    public virtual Type? FindTypeOrNull(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] Type candidateType)
    {
        if (!CheckCandidateTypeIsAView(candidateType))
            return null;

        if (!candidateType.IsConventional())
            return null;

        var typeByAttribute = LookupAttributedViewModelType(candidateType);
        if (typeByAttribute != null)
            return typeByAttribute;

        var concrete = LookupAssociatedConcreteViewModelType(candidateType);
        if (concrete != null)
            return concrete;

        var typeByName = LookupNamedViewModelType(candidateType);
        if (typeByName != null)
            return typeByName;

        MvxLogHost.Default?.Log(LogLevel.Warning, "No view model association found for candidate view {Name}", candidateType.Name);
        return null;
    }

    [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)]
    protected virtual Type? LookupAttributedViewModelType(Type candidateType)
    {
        var attribute = candidateType
            .GetCustomAttributes(typeof(MvxViewForAttribute), false)
            .FirstOrDefault() as MvxViewForAttribute;

        return attribute?.ViewModel;
    }

    [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)]
    protected virtual Type? LookupNamedViewModelType(Type candidateType)
    {
        var viewName = candidateType.Name;
        var viewModelName = viewToViewModelNameMapping.Map(viewName);

        viewModelByNameLookup.TryLookupByName(viewModelName, out Type? toReturn);
        return toReturn;
    }

    [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)]
    protected virtual Type? LookupAssociatedConcreteViewModelType(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] Type candidateType)
    {
        var viewModelPropertyInfo =
            Array.Find(candidateType.GetProperties(),
                x => x.Name == "ViewModel" &&
                     !x.PropertyType.GetTypeInfo().IsInterface &&
                     !x.PropertyType.GetTypeInfo().IsAbstract);

        return viewModelPropertyInfo?.PropertyType;
    }

    protected virtual bool CheckCandidateTypeIsAView(Type candidateType)
    {
        if (candidateType.GetTypeInfo().IsAbstract)
            return false;

        if (!typeof(IMvxView).IsAssignableFrom(candidateType))
            return false;

        return true;
    }
}
