// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.WeakSubscription;
using UIKit;

namespace MvvmCross.Platforms.Ios.Binding.Target
{
    public class MvxUISearchBarTextTargetBinding : MvxPropertyInfoTargetBinding<UISearchBar>
    {
        private IDisposable _subscription;

        public MvxUISearchBarTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var searchBar = View;
            if (searchBar == null)
            {
                MvxBindingLog.Error(
                                      "Error - UISearchBar is null in MvxUISearchBarTextTargetBinding");
                return;
            }

            _subscription = searchBar.WeakSubscribe<UISearchBar, UISearchBarTextChangedEventArgs>(nameof(searchBar.TextChanged), HandleSearchBarValueChanged);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            _subscription?.Dispose();
            _subscription = null;
        }

        private void HandleSearchBarValueChanged(object sender, UISearchBarTextChangedEventArgs e)
        {
            FireValueChanged(View.Text);
        }
    }
}
