// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.Base
{
#nullable enable
    public static class MvxApplicableExtensions
    {
        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static void Apply(this IEnumerable<IMvxApplicable> toApply)
        {
            if (toApply == null)
                throw new ArgumentNullException(nameof(toApply));

            foreach (var applicable in toApply)
                applicable.Apply();
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static void ApplyTo(this IEnumerable<IMvxApplicableTo> toApply, object what)
        {
            if (toApply == null)
                throw new ArgumentNullException(nameof(toApply));

            foreach (var applicable in toApply)
                applicable.ApplyTo(what);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public static void ApplyTo<T>(this IEnumerable<IMvxApplicableTo<T>> toApply, T what)
            where T : notnull
        {
            if (toApply == null)
                throw new ArgumentNullException(nameof(toApply));

            foreach (var applicable in toApply)
                applicable.ApplyTo(what);
        }
    }
#nullable restore
}
