// MvxNotifyChangeFieldSourceBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Cirrious.CrossCore;
using Cirrious.CrossCore.WeakSubscription;
using Cirrious.MvvmCross.Binding.Bindings.Source;
using Cirrious.MvvmCross.FieldBinding;

namespace MvvmCross.Plugins.FieldBinding
{
    public abstract class MvxNotifyChangeFieldSourceBinding
        : MvxSourceBinding
    {
        private static readonly EventInfo NotifyChangeEventInfo = typeof (INotifyChange).GetEvent("Changed");

        private readonly INotifyChange _notifyChange;
        private IDisposable _subscription;

        protected INotifyChange NotifyChange
        {
            get { return _notifyChange; }
        }

        protected MvxNotifyChangeFieldSourceBinding(object source, INotifyChange notifyChange)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (notifyChange == null)
                throw new ArgumentNullException("notifyChange");
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