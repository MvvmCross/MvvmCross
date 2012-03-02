#region Copyright
// <copyright file="MvxNullTargetBinding.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Bindings.Target
{
    public class MvxNullTargetBinding : MvxBaseTargetBinding
    {
        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneTime; }
        }

        public override Type TargetType
        {
            get { return typeof(Object); }
        }

        public override void SetValue(object value)
        {
            // ignored
        }
    }
}