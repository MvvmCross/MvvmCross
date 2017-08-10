using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Core.Views;

namespace MvvmCross.Droid.Views.Attributes
{
    public static class MvxAndroidPresentationAttributeExtensions
    {
        private static Type GetActivityViewModelType(Type activityType)
        {
            if (Mvx.TryResolve(out IMvxViewModelTypeFinder associatedTypeFinder))
                return associatedTypeFinder.FindTypeOrNull(activityType);

            MvxTrace.Trace("No view model type finder available - assuming we are looking for a splash screen - returning null");
            return typeof(MvxNullViewModel);
        }

        public static bool IsFragmentCacheable(this Type fragmentType, Type fragmentActivityParentType)
        {
            if (!fragmentType.HasBasePresentationAttribute())
                return false;

            var attribute = fragmentType.GetBasePresentationAttribute();
            if (attribute is MvxFragmentPresentationAttribute fragmentAttribute)
                return fragmentAttribute.IsCacheableFragment;

            return false;
        }
    }
}