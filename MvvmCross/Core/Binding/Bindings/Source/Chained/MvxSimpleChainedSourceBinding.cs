// MvxSimpleChainedSourceBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Source.Chained
{
    using System.Collections.Generic;
    using System.Reflection;

    using MvvmCross.Binding.Parse.PropertyPath.PropertyTokens;

    public class MvxSimpleChainedSourceBinding
        : MvxChainedSourceBinding
    {
        public MvxSimpleChainedSourceBinding(
            object source,
            PropertyInfo propertyInfo,
            IList<MvxPropertyToken> childTokens)
            : base(source, propertyInfo, childTokens)
        {
            this.UpdateChildBinding();
        }

        protected override object[] PropertyIndexParameters()
        {
            return null;
        }
    }
}