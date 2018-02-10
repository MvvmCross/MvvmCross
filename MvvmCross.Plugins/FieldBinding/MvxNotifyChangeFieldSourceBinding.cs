// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using MvvmCross.Binding.Bindings.Source;
using MvvmCross.WeakSubscription;

namespace MvvmCross.Plugin.FieldBinding
{
    public abstract class MvxNotifyChangeFieldSourceBinding
        : MvxSourceBinding
    {
        private static readonly EventInfo NotifyChangeEventInfo = typeof(INotifyChange).GetEvent("Changed");

        private readonly INotifyChange _notifyChange;
        private IDisposable _subscription;

        protected INotifyChange NotifyChange => _notifyChange;

        protected MvxNotifyChangeFieldSourceBinding(object source, INotifyChange notifyChange)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (notifyChange == null)
                throw new ArgumentNullException(nameof(notifyChange));
            _notifyChange = notifyChange;
            _subscription = NotifyChangeEventInfo.WeakSubscribe(_notifyChange, NotifyChangeOnChanged);
        }

        protected abstract void NotifyChangeOnChanged(object sender, EventArgs eventArgs);

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}
