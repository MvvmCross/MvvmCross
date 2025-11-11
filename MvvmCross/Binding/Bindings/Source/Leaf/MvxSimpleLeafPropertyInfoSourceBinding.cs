// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MvvmCross.Binding.Bindings.Source.Leaf
{
    [RequiresUnreferencedCode("This class uses reflection which may not be preserved during trimming")]
    public class MvxSimpleLeafPropertyInfoSourceBinding(object source, PropertyInfo propertyInfo)
        : MvxLeafPropertyInfoSourceBinding(source, propertyInfo)
    {
        protected override object[] PropertyIndexParameters() => [];
    }
}
