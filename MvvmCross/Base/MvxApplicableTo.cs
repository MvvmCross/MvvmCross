// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Base
{
#nullable enable
    public abstract class MvxApplicableTo<T>
        : MvxApplicable,
          IMvxApplicableTo<T>
        where T : notnull
    {
        public virtual void ApplyTo(T what)
        {
            SuppressFinalizer();
        }
    }
#nullable restore
}
