using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Core.Views;
using Android.App;
using MvvmCross.Platform.Droid.Platform;

namespace MvvmCross.Droid.Views.Attributes
{
    public static class MvxAndroidPresentationAttributeExtensions
    {
        private static Type GetActivityViewModelType(Type activityType)
        {
            IMvxViewModelTypeFinder associatedTypeFinder;
            if (!Mvx.TryResolve(out associatedTypeFinder))
            {
                MvxTrace.Trace("No view model type finder available - assuming we are looking for a splash screen - returning null");
                return typeof(MvxNullViewModel);
            }

            return associatedTypeFinder.FindTypeOrNull(activityType);
        }

        public static bool IsFragmentCacheable(this Type fragmentType, Type fragmentActivityParentType)
        {
            if (!fragmentType.HasBasePresentationAttribute())
                return false;

            var attribute = fragmentType.GetBasePresentationAttribute();
            if (attribute is MvxFragmentPresentationAttribute fragmentAttribute)
                return fragmentAttribute.IsCacheableFragment;
            else
                return false;
        }
    }
}