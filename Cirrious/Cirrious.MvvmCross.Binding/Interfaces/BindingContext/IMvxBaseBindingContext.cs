// IMvxBaseBindingContext.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Droid.Interfaces.BindingContext
{
    public interface IMvxBaseBindingContext<TViewType>
        : IMvxDataConsumer
    {
        void RegisterBindingsFor(TViewType view, IList<IMvxUpdateableBinding> bindings);
        void RegisterBinding(IMvxUpdateableBinding binding);
        void ClearBindings(TViewType view);
        void ClearAllBindings();
    }
}