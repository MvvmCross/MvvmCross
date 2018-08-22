// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace MvvmCross.Binding.Bindings.Source.Leaf
{
    public class MvxSimpleLeafPropertyInfoSourceBinding : MvxLeafPropertyInfoSourceBinding
    {
        public MvxSimpleLeafPropertyInfoSourceBinding(object source, PropertyInfo propertyInfo)
            : base(source, propertyInfo)
        {
        }

        protected override object[] PropertyIndexParameters()
        {
            return null;
        }
    }
}