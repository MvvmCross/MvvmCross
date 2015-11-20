// MvxSimpleChainedSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;
using System.Collections.Generic;
using System.Reflection;

namespace Cirrious.MvvmCross.Binding.Bindings.Source.Chained
{
    public class MvxSimpleChainedSourceBinding
        : MvxChainedSourceBinding
    {
        public MvxSimpleChainedSourceBinding(
            object source,
            PropertyInfo propertyInfo,
            IList<MvxPropertyToken> childTokens)
            : base(source, propertyInfo, childTokens)
        {
            UpdateChildBinding();
        }

        protected override object[] PropertyIndexParameters()
        {
            return null;
        }
    }
}