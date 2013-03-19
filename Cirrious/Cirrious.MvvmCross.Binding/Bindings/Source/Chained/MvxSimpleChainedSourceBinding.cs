// MvxSimpleChainedSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

namespace Cirrious.MvvmCross.Binding.Bindings.Source.Chained
{
    public class MvxSimpleChainedSourceBinding
        : MvxChainedSourceBinding
    {
        public MvxSimpleChainedSourceBinding(
            object source,
            MvxPropertyNamePropertyToken propertyNamePropertyToken,
            IList<MvxPropertyToken> childTokens)
            : base(source, propertyNamePropertyToken.PropertyName, childTokens)
        {
            UpdateChildBinding();
        }

        protected override object[] PropertyIndexParameters()
        {
            return null;
        }
    }
}