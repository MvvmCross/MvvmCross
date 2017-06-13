// MvxMissingSourceBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Platform.Converters;

namespace MvvmCross.Binding.Bindings.Source
{
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