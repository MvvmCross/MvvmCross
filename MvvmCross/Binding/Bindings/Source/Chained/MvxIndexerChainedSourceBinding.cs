// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace MvvmCross.Binding.Bindings.Source.Chained
{
    [RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
    public class MvxIndexerChainedSourceBinding
        : MvxChainedSourceBinding
    {
        private readonly MvxIndexerPropertyToken _indexerPropertyToken;

        public MvxIndexerChainedSourceBinding(object source, PropertyInfo itemPropertyInfo, MvxIndexerPropertyToken indexerPropertyToken,
                                                  IList<IMvxPropertyToken> childTokens)
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
