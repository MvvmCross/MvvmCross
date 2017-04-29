// MvxNotifyChangeFieldSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using MvvmCross.Binding.Bindings.Source;
using MvvmCross.FieldBinding;
using MvvmCross.Platform;
using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Plugins.FieldBinding
{
    public abstract class MvxNotifyChangeFieldSourceBinding
        : MvxSourceBinding
    {
        private static readonly EventInfo NotifyChangeEventInfo = typeof(INotifyChange).GetEvent("Changed");

        private readonly IDisposable _subscription;

        protected MvxNotifyChangeFieldSourceBinding(object source, INotifyChange notifyChange)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (notifyChange == null)
                throw new ArgumentNullException(nameof(notifyChange));
            NotifyChange = notifyChange;
            _subscription = NotifyChangeEventInfo.WeakSubscribe(NotifyChange, NotifyChangeOnChanged);
        }

        protected INotifyChange NotifyChange { get; }

        protected abstract void NotifyChangeOnChanged(object sender, EventArgs eventArgs);

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
                _subscription.Dispose();

            base.Dispose(isDisposing);
        }
    }
}