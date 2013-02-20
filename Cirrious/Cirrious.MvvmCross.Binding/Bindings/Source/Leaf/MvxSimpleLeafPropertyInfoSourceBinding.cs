// MvxSimpleLeafPropertyInfoSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace Cirrious.MvvmCross.Binding.Bindings.Source.Leaf
{
    public class MvxSimpleLeafPropertyInfoSourceBinding : MvxLeafPropertyInfoSourceBinding
    {
        public MvxSimpleLeafPropertyInfoSourceBinding(object source, MvxPropertyNamePropertyToken token)
            : base(source, token.PropertyName)
        {
        }

        protected override object[] PropertyIndexParameters()
        {
            return null;
        }
    }
}