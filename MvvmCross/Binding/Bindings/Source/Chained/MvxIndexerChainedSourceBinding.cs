// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace MvvmCross.Binding.Bindings.Source.Chained
{
    public class MvxIndexerChainedSourceBinding
        : MvxChainedSourceBinding
    {
        private readonly MvxIndexerPropertyToken _indexerPropertyToken;

        public MvxIndexerChainedSourceBinding(object source, PropertyInfo itemPropertyInfo, MvxIndexerPropertyToken indexerPropertyToken,
                                                  IList<MvxPropertyToken> childTokens)
            : base(source, itemPropertyInfo, childTokens)
        {
            _indexerPropertyToken = indexerPropertyToken;
            UpdateChildBinding();
        }

        protected override object[] PropertyIndexParameters()
        {
            return new[] { _indexerPropertyToken.Key };
        }
    }
}
