// MvxIndexerChainedSourceBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Source.Chained
{
    using System.Collections.Generic;
    using System.Reflection;

    using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

    public class MvxIndexerChainedSourceBinding
        : MvxChainedSourceBinding
    {
        private readonly MvxIndexerPropertyToken _indexerPropertyToken;

        public MvxIndexerChainedSourceBinding(object source, PropertyInfo itemPropertyInfo, MvxIndexerPropertyToken indexerPropertyToken,
                                                  IList<MvxPropertyToken> childTokens)
            : base(source, itemPropertyInfo, childTokens)
        {
            this._indexerPropertyToken = indexerPropertyToken;
            this.UpdateChildBinding();
        }

        protected override object[] PropertyIndexParameters()
        {
            return new[] { this._indexerPropertyToken.Key };
        }
    }
}