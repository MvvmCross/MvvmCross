// MvxIndexerLeafPropertyInfoSourceBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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