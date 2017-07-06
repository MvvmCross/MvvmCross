using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Droid.Views.Attributes
{
    public static class MvxFragmentAttributeExtensionMethods
    {
        //TODO: Move to core
        public static bool HasBasePresentationAttribute(this Type candidateType)
        {
            var attributes = candidateType.GetCustomAttributes(typeof(MvxBasePresentationAttribute), true);
            return attributes.Length > 0;
        }

        public static IEnumerable<MvxBasePresentationAttribute> GetBasePresentationAttributes(this Type fromFragmentType)
        {
            var attributes = fromFragmentType.GetCustomAttributes(typeof(MvxBasePresentationAttribute), true);

            if (!attributes.Any())
                throw new InvalidOperationException($"Type does not have {nameof(MvxBasePresentationAttribute)} attribute!");

            return attributes.Cast<MvxBasePresentationAttribute>();
        }

        public static MvxBasePresentationAttribute GetBasePresentationAttribute(this Type fromFragmentType,
            Type fragmentActivityParentType)
        {
            var mvxFragmentAttributes = fromFragmentType.GetBasePresentationAttributes();
            var activityViewModelType = GetActivityViewModelType(fragmentActivityParentType);
            var mvxFragmentAttribute = mvxFragmentAttributes.FirstOrDefault();

            if (mvxFragmentAttribute == null)
                throw new InvalidOperationException($"Sorry but Fragment Type: {fromFragmentType} hasn't registered any Activity with ViewModel Type {fragmentActivityParentType}");

            return mvxFragmentAttribute;
        }

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

            var attribute = fragmentType.GetBasePresentationAttribute(fragmentActivityParentType);

            if (attribute is MvxFragmentAttribute fragmentAttribute)
                return fragmentAttribute.IsCacheableFragment;
            else
                return false;
        }

        public static Type GetViewModelType(this Type fragmentType)
        {
            if (!fragmentType.HasBasePresentationAttribute())
                return null;

            return fragmentType.GetBasePresentationAttributes()
                               .OfType<MvxFragmentAttribute>()
                .Select(x => x.ViewModelType)
                .First();
        }
    }
}