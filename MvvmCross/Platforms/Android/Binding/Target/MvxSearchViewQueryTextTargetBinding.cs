// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxSearchViewQueryTextTargetBinding
        : MvxAndroidTargetBinding
    {
        public MvxSearchViewQueryTextTargetBinding(object target)
            : base(target)
        {
        }

        private IDisposable _subscription;

        public override Type TargetValueType => typeof(string);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        protected SearchView SearchView => (SearchView)Target;

        public override void SubscribeToEvents()
        {
            _subscription = SearchView.WeakSubscribe<SearchView, SearchView.QueryTextChangeEventArgs>(
                nameof(SearchView.QueryTextChange),
                HandleQueryTextChanged);
        }

        protected override void SetValueImpl(object target, object value) =>
            ((SearchView)target).SetQuery((string)value, true);

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
                _subscription = null;
            }

            base.Dispose(isDisposing);
        }

        private void HandleQueryTextChanged(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            var target = Target as SearchView;

            if (target == null)
            {
                return;
            }

            var value = target.Query;
            FireValueChanged(value);
        }
    }
}
