// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MvvmCross.ViewModels
{
#nullable enable
    public interface IMvxTypeToTypeLookupBuilder
    {
        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        IDictionary<Type, Type> Build(IEnumerable<Assembly> sourceAssemblies);
    }
#nullable restore
}
