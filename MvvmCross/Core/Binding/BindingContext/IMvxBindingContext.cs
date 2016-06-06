// IMvxBindingContext.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.BindingContext
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Binding.Bindings;
    using MvvmCross.Platform.Core;

    public interface IMvxBindingContext
        : IMvxDataConsumer
    {
        event EventHandler DataContextChanged;

        IMvxBindingContext Init(object dataContext, object firstBindingKey, IEnumerable<MvxBindingDescription> firstBindingValue);

        IMvxBindingContext Init(object dataContext, object firstBindingKey, string firstBindingValue);

        void RegisterBinding(object target, IMvxUpdateableBinding binding);

        void RegisterBindingsWithClearKey(object clearKey, IEnumerable<KeyValuePair<object, IMvxUpdateableBinding>> bindings);

        void RegisterBindingWithClearKey(object clearKey, object target, IMvxUpdateableBinding binding);

        void ClearBindings(object clearKey);

        void ClearAllBindings();

        void DelayBind(Action action);
    }
}