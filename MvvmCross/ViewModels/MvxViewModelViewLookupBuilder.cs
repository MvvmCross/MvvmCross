// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MvvmCross.Exceptions;
using MvvmCross.IoC;

namespace MvvmCross.ViewModels;

public class MvxViewModelViewLookupBuilder
    : IMvxTypeToTypeLookupBuilder
{
    [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
    public virtual IDictionary<Type, Type> Build(IEnumerable<Assembly> sourceAssemblies)
    {
        var associatedTypeFinder = Mvx.IoCProvider?.Resolve<IMvxViewModelTypeFinder>();

        var views = sourceAssemblies
            .SelectMany(assembly => assembly.ExceptionSafeGetTypes(),
                (assembly, candidateViewType) => new { assembly, candidateViewType })
            .Select(t => new { t, viewModelType = associatedTypeFinder?.FindTypeOrNull(t.candidateViewType) })
            .Where(t => t.viewModelType != null)
            .Select(t => (t.viewModelType, t.t.candidateViewType));

        var filteredViews = FilterViews(views);

        try
        {
            return filteredViews?.ToDictionary(x => x.Item1, x => x.Item2) ?? new Dictionary<Type, Type>();
        }
        catch (ArgumentException exception)
        {
            throw ReportBuildProblem(views, exception);
        }
    }

    protected virtual IEnumerable<(Type, Type)>? FilterViews(IEnumerable<(Type, Type)>? views)
    {
        return views;
    }

    protected virtual Exception ReportBuildProblem(
        IEnumerable<(Type, Type)> views, ArgumentException exception)
    {
        var overSizedCounts =
            views.GroupBy(x => x.Item1)
                .Select(x => new { x.Key.Name, Count = x.Count(), ViewNames = x.Select(v => v.Item2.Name).ToArray() })
                .Where(x => x.Count > 1)
                .Select(x => $"{x.Count}*{x.Name} ({string.Join(",", x.ViewNames)})")
                .ToArray();

        if (overSizedCounts.Length == 0)
        {
            // no idea what the error is - so throw the original
            return exception.MvxWrap("Unknown problem in ViewModelViewLookup construction");
        }

        var overSizedText = string.Join(";", overSizedCounts);
        return exception.MvxWrap(
            "Problem seen creating View-ViewModel lookup table - you have more than one View registered for the ViewModels: {0}",
            overSizedText);
    }
}
