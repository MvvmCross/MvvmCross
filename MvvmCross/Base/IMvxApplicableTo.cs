// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.Base
{
#nullable enable
    public interface IMvxApplicableTo
    {
        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        void ApplyTo(object what);
    }

    public interface IMvxApplicableTo<in T>
        where T : notnull
    {
        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        void ApplyTo(T what);
    }
#nullable restore
}
