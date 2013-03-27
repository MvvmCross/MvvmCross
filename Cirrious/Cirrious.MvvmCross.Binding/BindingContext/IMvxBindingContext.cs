// IMvxBindingContext.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Binding.Bindings;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public interface IMvxBindingContext
        : IMvxDataConsumer
    {
        void RegisterBindingsFor(object target, IList<IMvxUpdateableBinding> bindings);
        void RegisterBinding(IMvxUpdateableBinding binding);
        void ClearBindings(object target);
        void ClearAllBindings();
        void DelayBind(Action action);
    }
}