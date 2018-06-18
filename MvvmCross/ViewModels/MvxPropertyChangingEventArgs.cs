// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

namespace MvvmCross.ViewModels
{
    public class MvxPropertyChangingEventArgs<T> : PropertyChangingEventArgs
    {
        public MvxPropertyChangingEventArgs(string propertyName, T newValue) : base(propertyName)
        {
            NewValue = newValue;
        }

        public bool Cancel { get; set; }
        public T NewValue { get; set; }
    }
}
