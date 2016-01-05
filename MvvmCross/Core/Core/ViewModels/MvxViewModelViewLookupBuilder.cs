// MvxViewModelViewLookupBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.IoC;

    public class MvxViewModelViewLookupBuilder
        : IMvxTypeToTypeLookupBuilder
    {
        public virtual IDictionary<Type, Type> Build(IEnumerable<Assembly> sourceAssemblies)
        {
            var associatedTypeFinder = Mvx.Resolve<IMvxViewModelTypeFinder>();

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