// MvxTargetChangedEventArgs.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Target
{
    using System;

    public class MvxTargetChangedEventArgs
        : EventArgs
    {
        public MvxTargetChangedEventArgs(object value)
        {
            this.Value = value;
        }

        public Object Value { get; private set; }
    }
}