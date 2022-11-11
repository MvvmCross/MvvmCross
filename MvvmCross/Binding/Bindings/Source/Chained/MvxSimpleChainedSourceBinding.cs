// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace MvvmCross.Binding.Bindings.Source.Chained
{
    public class MvxSimpleChainedSourceBinding
        : MvxChainedSourceBinding
    {
        public MvxSimpleChainedSourceBinding(
            object source,
            PropertyInfo propertyInfo,
            IList<MvxPropertyToken> childTokens)
            : base(source, propertyInfo, childTokens)
        {
            UpdateChildBinding();
        }

        protected override object[] PropertyIndexParameters()
        {
            return Array.Empty<object>();
        }
    }
}
