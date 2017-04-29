// MvxUISearchBarTextTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Reflection;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using UIKit;

namespace MvvmCross.Binding.tvOS.Target
{
    public class MvxUISearchBarTextTargetBinding : MvxPropertyInfoTargetBinding<UISearchBar>
    {
        public MvxUISearchBarTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var searchBar = View;
            if (searchBar == null)
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                    "Error - UISearchBar is null in MvxUISearchBarTextTargetBinding");
            else
                searchBar.TextChanged += HandleSearchBarValueChanged;
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        private void HandleSearchBarValueChanged(object sender, UISearchBarTextChangedEventArgs e)
        {
            FireValueChanged(View.Text);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var searchBar = View;
                if (searchBar != null)
                    searchBar.TextChanged -= HandleSearchBarValueChanged;
            }
        }
    }
}