// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MvvmCross.Base
{
#nullable enable
    public interface IMvxNamedInstanceRegistry<in T>
        where T : notnull
    {
        void AddOrOverwrite(string name, T instance);

        [RequiresUnreferencedCode("Gets types from assembly")]
        void AddOrOverwriteFrom(Assembly assembly);
    }
#nullable restore
}
