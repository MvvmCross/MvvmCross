// MvxValueEventArgs.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.CrossCore.Core
{
    public class MvxValueEventArgs<T>
        : EventArgs
    {
        public MvxValueEventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }
}