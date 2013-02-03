// IMvxBindingTouchView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Touch.Interfaces;

namespace Cirrious.MvvmCross.Binding.Touch.Interfaces
{
#warning Should IMvxBindingTouchView inherit from IMvxTouchView ?
    public interface IMvxBindingTouchView
		: IMvxTouchView
    {
        List<IMvxUpdateableBinding> Bindings { get; }
    }
}