// MvxMissingSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Source
{
    using System;

    using MvvmCross.Platform.Converters;

    public class MvxMissingSourceBinding : MvxSourceBinding
    {
        public MvxMissingSourceBinding(object source) : base(source)
        {
        }

        public override void SetValue(object value)
        {
            // nothing we can do here - binding is missing
        }

        public override Type SourceType => typeof(object);

        public override object GetValue()
        {
            // binding is missing so return 'unset value'
            return MvxBindingConstant.UnsetValue;
        }
    }
}