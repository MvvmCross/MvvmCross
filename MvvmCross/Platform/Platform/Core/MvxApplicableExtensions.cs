// MvxApplicableExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;

namespace Cirrious.CrossCore.Core
{
    public static class MvxApplicableExtensions
    {
        public static void Apply(this IEnumerable<IMvxApplicable> toApply)
        {
            foreach (var applicable in toApply)
                applicable.Apply();
        }

        public static void ApplyTo(this IEnumerable<IMvxApplicableTo> toApply, object what)
        {
            foreach (var applicable in toApply)
                applicable.ApplyTo(what);
        }

        public static void ApplyTo<T>(this IEnumerable<IMvxApplicableTo<T>> toApply, T what)
        {
            foreach (var applicable in toApply)
                applicable.ApplyTo(what);
        }
    }
}