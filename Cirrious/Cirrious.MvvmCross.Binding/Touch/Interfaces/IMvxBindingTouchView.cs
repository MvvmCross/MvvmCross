#region Copyright
// <copyright file="IMvxBindingTouchView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Binding.Touch.Interfaces
{
    public interface IMvxBindingTouchView
        : IMvxServiceConsumer<IMvxBinder>
    {
        List<IMvxUpdateableBinding> Bindings { get; }
        object DefaultBindingSource { get; }
    }
}