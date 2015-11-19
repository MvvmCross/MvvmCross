// MvxUISearchBarTextTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Reflection;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Target
{
    public class MvxUISearchBarTextTargetBinding : MvxPropertyInfoTargetBinding<UISearchBar>
    {
        public MvxUISearchBarTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var searchBar = View;
            if (searchBar == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - UISearchBar is null in MvxUISearchBarTextTargetBinding");
            }
            else
            {
                searchBar.TextChanged += HandleSearchBarValueChanged;
            }
        }

        private void HandleSearchBarValueChanged(object sender, UISearchBarTextChangedEventArgs e)
        {
            FireValueChanged(View.Text);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var searchBar = View;
                if (searchBar != null)
                {
                    searchBar.TextChanged -= HandleSearchBarValueChanged;
                }
            }
        }
    }
}