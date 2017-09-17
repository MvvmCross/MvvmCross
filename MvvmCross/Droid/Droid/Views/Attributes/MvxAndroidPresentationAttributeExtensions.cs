using System;
using System.Linq;
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

            var fragmentAttributes = fragmentType.GetBasePresentationAttributes()
                                                 .Select(baseAttribute => baseAttribute as MvxFragmentPresentationAttribute)
                                                 .Where(fragmentAttribute => fragmentAttribute != null);

            var currentAttribute = fragmentAttributes.FirstOrDefault(fragmentAttribute => fragmentAttribute.ActivityHostViewModelType == fragmentActivityParentType);

            return currentAttribute != null ? currentAttribute.IsCacheableFragment : false;
        }
    }
}