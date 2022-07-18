// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Views;
using MvvmCross.Binding.Extensions;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxViewHiddenBinding
        : MvxBaseViewVisibleBinding
    {
        public MvxViewHiddenBinding(object target)
            : base(target)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            ((View)target).Visibility = value.ConvertToBoolean() ? ViewStates.Gone : ViewStates.Visible;
        }
    }
}
