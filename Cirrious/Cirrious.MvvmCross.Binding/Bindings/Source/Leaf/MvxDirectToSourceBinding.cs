// MvxDirectToSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Binding.Bindings.Source.Leaf
{
    public class MvxDirectToSourceBinding : MvxSourceBinding
    {
        public MvxDirectToSourceBinding(object source)
            : base(source)
        {
        }

        public override Type SourceType
        {
            get { return Source == null ? typeof (object) : Source.GetType(); }
        }

        public override void SetValue(object value)
        {
            MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                  "ToSource binding is not available for direct pathed source bindings");
        }

        public override bool TryGetValue(out object value)
        {
            value = Source;
            return true;
        }
    }
}