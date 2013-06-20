using System.Reflection;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using MonoTouch.UIKit;

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

        private void HandleSearchBarValueChanged(object sender, System.EventArgs e)
        {
            FireValueChanged(View.Text);
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

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
