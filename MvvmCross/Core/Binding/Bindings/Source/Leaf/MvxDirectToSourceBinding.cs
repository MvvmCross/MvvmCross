// MvxDirectToSourceBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Source.Leaf
{
    using System;

    using MvvmCross.Platform.Platform;

    public class MvxDirectToSourceBinding : MvxSourceBinding
    {
        public MvxDirectToSourceBinding(object source)
            : base(source)
        {
        }

        public override Type SourceType => this.Source == null ? typeof(object) : this.Source.GetType();

        public override void SetValue(object value)
        {
            MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                  "ToSource binding is not available for direct pathed source bindings");
        }

        public override object GetValue()
        {
            return this.Source;
        }
    }
}