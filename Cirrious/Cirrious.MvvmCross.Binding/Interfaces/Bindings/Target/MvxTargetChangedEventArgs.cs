#region Copyright
// <copyright file="MvxTargetChangedEventArgs.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;

namespace Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target
{
    public class MvxTargetChangedEventArgs 
        : EventArgs
    {
        public MvxTargetChangedEventArgs(object value)
        {
            Value = value;
        }

        public Object Value { get; private set; }
    }
}