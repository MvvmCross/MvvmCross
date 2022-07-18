// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Binding.Bindings.Target
{
    public class MvxNullTargetBinding : MvxTargetBinding
    {
        public MvxNullTargetBinding()
            : base(null)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneTime;

        public override Type TargetValueType => typeof(object);

        public override void SetValue(object value)
        {
            // ignored
        }
    }
}
