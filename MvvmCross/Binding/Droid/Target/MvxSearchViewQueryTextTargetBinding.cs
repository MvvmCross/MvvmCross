using System;
using System.Reflection;
using Android.Widget;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxSearchViewQueryTextTargetBinding 
        : MvxWithEventPropertyInfoTargetBinding<SearchView>
    {
        public MvxSearchViewQueryTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            EventSuffix = "TextChange";

            var searchView = View;
            if (searchView == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - searchView is null in MvxSearchViewQueryTextTargetBinding");
            }
        }

        public override Type TargetType => typeof(string);
    }
}