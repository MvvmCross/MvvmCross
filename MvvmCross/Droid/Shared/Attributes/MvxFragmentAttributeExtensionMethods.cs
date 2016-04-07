// MvxConventionAttributeExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;

namespace MvvmCross.Droid.Shared.Attributes
{
    public static class MvxFragmentAttributeExtensionMethods
    {
        public static bool HasMvxFragmentAttribute(this Type candidateType)
        {
            var attributes = candidateType.GetCustomAttributes(typeof(MvxFragmentAttribute), true);
            return attributes.Length > 0;
        }

        public static IEnumerable<MvxFragmentAttribute> GetMvxFragmentAttributes(this Type fromFragmentType)
        {
            var attributes = fromFragmentType.GetCustomAttributes(typeof(MvxFragmentAttribute), true);

            if (!attributes.Any())
                throw new InvalidOperationException($"Type does not have {nameof(MvxFragmentAttribute)} attribute!");

            return attributes.Cast<MvxFragmentAttribute>();
        }

        public static MvxFragmentAttribute GetMvxFragmentAttribute(this Type fromFragmentType,
            Type fragmentActivityViewModelType)
        {
            var mvxFragmentAttributes = fromFragmentType.GetMvxFragmentAttributes();

           var mvxFragmentAttribute = mvxFragmentAttributes.FirstOrDefault(x => x.ParentActivityViewModelType == fragmentActivityViewModelType);

            if (mvxFragmentAttributes == null)
                throw new InvalidOperationException($"Sorry but Fragment Type: {fromFragmentType} hasn't registered any Activity with ViewModel Type {fragmentActivityViewModelType}");

            return mvxFragmentAttribute;
        }

        public static bool IsFragmentCacheable(this Type fragmentType, Type fragmentActivityParentType)
        {
            if (!fragmentType.HasMvxFragmentAttribute())
                return false;

            var mvxFragmentAttribute = fragmentType.GetMvxFragmentAttribute(fragmentActivityParentType);
            return mvxFragmentAttribute.IsCacheableFragment;
        }

        public static Type GetViewModelType(this Type fragmentType)
        {
            if (!fragmentType.HasMvxFragmentAttribute())
                return null;

            return fragmentType.GetMvxFragmentAttributes()
                .Select(x => x.ViewModelType)
                .First();
        }
    }
}