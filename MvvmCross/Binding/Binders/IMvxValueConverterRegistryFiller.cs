// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MvvmCross.Base;
using MvvmCross.Converters;

namespace MvvmCross.Binding.Binders
{
    public interface IMvxNamedInstanceRegistryFiller<out T>
    {
        string FindName(Type type);

        void FillFrom(
            IMvxNamedInstanceRegistry<T> registry,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)] Type type);

        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        void FillFrom(IMvxNamedInstanceRegistry<T> registry, Assembly assembly);
    }

    public interface IMvxValueConverterRegistryFiller : IMvxNamedInstanceRegistryFiller<IMvxValueConverter>
    {
    }
}
