// MvxUISearchBarTextTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;
using UIKit;

namespace MvvmCross.Binding.iOS.Target
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