// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace MvvmCross.Base
{
#nullable enable
    public static class MvxApplicableExtensions
    {
        public static void Apply(this IEnumerable<IMvxApplicable> toApply)
        {
            if (toApply == null)
                throw new ArgumentNullException(nameof(toApply));

            foreach (var applicable in toApply)
                applicable.Apply();
        }

        public static void ApplyTo(this IEnumerable<IMvxApplicableTo> toApply, object what)
        {
            if (toApply == null)
                throw new ArgumentNullException(nameof(toApply));

            foreach (var applicable in toApply)
                applicable.ApplyTo(what);
        }

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
