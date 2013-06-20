// MvxPathIndexerLeafPropertyInfoSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace Cirrious.MvvmCross.Binding.Bindings.PathSource.Leaf
{
    public class MvxPathIndexerLeafPropertyInfoSourceBinding : MvxPathLeafPropertyInfoSourceBinding
    {
        private readonly object _key;

        public MvxPathIndexerLeafPropertyInfoSourceBinding(object source, MvxIndexerPropertyToken indexToken)
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