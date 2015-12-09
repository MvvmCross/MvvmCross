// MvxUISearchBarTextTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


#if __UNIFIED__
using AppKit;
using Foundation;
using ObjCRuntime;
#else
#endif

namespace MvvmCross.Binding.Mac.Target
{
    using System.Reflection;

    using global::MvvmCross.Platform.Platform;

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
                searchField.Action = new Selector("searchFieldAction:");
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