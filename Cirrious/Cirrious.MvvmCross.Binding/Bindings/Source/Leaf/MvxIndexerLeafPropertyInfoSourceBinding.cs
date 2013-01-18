// MvxIndexerLeafPropertyInfoSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Bindings.Source.Construction.PropertyTokens;

namespace Cirrious.MvvmCross.Binding.Bindings.Source.Leaf
{
    public class MvxIndexerLeafPropertyInfoSourceBinding : MvxLeafPropertyInfoSourceBinding
    {
        private readonly object _key;

        public MvxIndexerLeafPropertyInfoSourceBinding(object source, MvxIndexerPropertyToken indexToken)
            : base(source, "Item")
        {
            _key = indexToken.Key;
        }

        protected override object[] PropertyIndexParameters()
        {
            return new[] {_key};
        }
    }
}