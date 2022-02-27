// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace MvvmCross.Binding.Bindings.Source.Leaf
{
    public class MvxIndexerLeafPropertyInfoSourceBinding : MvxLeafPropertyInfoSourceBinding
    {
        private readonly object _key;

        public MvxIndexerLeafPropertyInfoSourceBinding(object source, PropertyInfo itemPropertyInfo, MvxIndexerPropertyToken indexToken)
            : base(source, itemPropertyInfo)
        {
            _key = indexToken.Key;
        }

        protected override object[] PropertyIndexParameters()
        {
            return new[] { _key };
        }
    }
}
