// MvxIndexerLeafPropertyInfoSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
using System.Reflection;

namespace Cirrious.MvvmCross.Binding.Bindings.Source.Leaf
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