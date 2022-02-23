// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Binding.Bindings.Source.Leaf
{
    public class MvxDirectToSourceBinding : MvxSourceBinding
    {
        public MvxDirectToSourceBinding(object source)
            : base(source)
        {
        }

        public override Type SourceType => Source == null ? typeof(object) : Source.GetType();

        public override void SetValue(object value)
        {
            MvxBindingLog.Warning("ToSource binding is not available for direct pathed source bindings");
        }

        public override object GetValue()
        {
            return Source;
        }
    }
}
