// MvxUISearchBarTextTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Reflection;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using MonoMac.AppKit;
using MonoMac.Foundation;

namespace Cirrious.MvvmCross.Binding.Mac.Target
{
#warning Unlikley this works!
    public class MvxNSSearchFieldTextTargetBinding : MvxPropertyInfoTargetBinding<NSSearchField>
    {
		public MvxNSSearchFieldTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var searchField = View;
            if (searchField == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
				                      "Error - NSSearchField is null in MvxNSSearchFieldTextTargetBinding");
            }
            else
            {
				searchField.Action = new MonoMac.ObjCRuntime.Selector ("searchFieldAction:");
            }
        }

		[Export("searchFieldAction:")]
        private void searchFieldAction()
        {
            FireValueChanged(View.StringValue);
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
//                    searchBar.TextChanged -= HandleSearchBarValueChanged;
                }
            }
        }
    }
}