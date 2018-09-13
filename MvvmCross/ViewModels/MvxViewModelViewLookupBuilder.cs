// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Exceptions;
using MvvmCross.IoC;

namespace MvvmCross.ViewModels
{
    public class MvxViewModelViewLookupBuilder
        : IMvxTypeToTypeLookupBuilder
    {
        public virtual IDictionary<Type, Type> Build(IEnumerable<Assembly> sourceAssemblies)
        {
            var associatedTypeFinder = Mvx.IoCProvider.Resolve<IMvxViewModelTypeFinder>();

            var views = from assembly in sourceAssemblies
                        from candidateViewType in assembly.ExceptionSafeGetTypes()
                        let viewModelType = associatedTypeFinder.FindTypeOrNull(candidateViewType)
                        where viewModelType != null
                        select new KeyValuePair<Type, Type>(viewModelType, candidateViewType);

            try
            {
                return views.ToDictionary(x => x.Key, x => x.Value);
            }
            catch (ArgumentException exception)
            {
                throw ReportBuildProblem(views, exception);
            }
        }

        private static Exception ReportBuildProblem(IEnumerable<KeyValuePair<Type, Type>> views,
                                                    ArgumentException exception)
        {
            var overSizedCounts = views.GroupBy(x => x.Key)
                                       .Select(x => new { x.Key.Name, Count = x.Count(), ViewNames = x.Select(v => v.Value.Name).ToList() })
                                       .Where(x => x.Count > 1)
                                       .Select(x => $"{x.Count}*{x.Name} ({string.Join(",", x.ViewNames)})")
                                       .ToArray();

            if (overSizedCounts.Length == 0)
            {
                // no idea what the error is - so throw the original
                return exception.MvxWrap("Unknown problem in ViewModelViewLookup construction");
            }
            else
            {
                var overSizedText = string.Join(";", overSizedCounts);
                return exception.MvxWrap(
                    "Problem seen creating View-ViewModel lookup table - you have more than one View registered for the ViewModels: {0}",
                    overSizedText);
            }
        }
    }
}