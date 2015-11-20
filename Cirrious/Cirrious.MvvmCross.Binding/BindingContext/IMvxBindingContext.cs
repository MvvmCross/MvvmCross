// IMvxBindingContext.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Binding.Bindings;
using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Binding.BindingContext
{
    public interface IMvxBindingContext
        : IMvxDataConsumer
    {
        event EventHandler DataContextChanged;

        void RegisterBinding(object target, IMvxUpdateableBinding binding);

        void RegisterBindingsWithClearKey(object clearKey, IEnumerable<KeyValuePair<object, IMvxUpdateableBinding>> bindings);

        void RegisterBindingWithClearKey(object clearKey, object target, IMvxUpdateableBinding binding);

        void ClearBindings(object clearKey);

        void ClearAllBindings();

        void DelayBind(Action action);
    }
}