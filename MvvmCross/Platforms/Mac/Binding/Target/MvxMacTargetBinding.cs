// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Platforms.Mac.Binding.Target
{
    public abstract class MvxMacTargetBinding : MvxConvertingTargetBinding
    {
        protected MvxMacTargetBinding(object view)
            : base(view)
        {
        }
    }
}
