// MvxSimpleLeafPropertyInfoSourceBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Source.Leaf
{
    using System.Reflection;

    public class MvxSimpleLeafPropertyInfoSourceBinding : MvxLeafPropertyInfoSourceBinding
    {
        public MvxSimpleLeafPropertyInfoSourceBinding(object source, PropertyInfo propertyInfo)
            : base(source, propertyInfo)
        {
        }

        protected override object[] PropertyIndexParameters()
        {
            return null;
        }
    }
}