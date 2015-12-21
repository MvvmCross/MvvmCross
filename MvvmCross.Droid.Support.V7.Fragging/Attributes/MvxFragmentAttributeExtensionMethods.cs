// MvxConventionAttributeExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;

namespace MvvmCross.Droid.Support.V7.Fragging.Attributes
{
    public static class MvxFragmentAttributeExtensionMethods
    {
        public static bool HasMvxFragmentAttribute(this Type candidateType)
        {
            var attributes = candidateType.GetCustomAttributes(typeof(MvxFragmentAttribute), true);
            return attributes.Length > 0;
        }

        public static MvxFragmentAttribute GetMvxFragmentAttribute(this Type fromFragmentType)
        {
            var attributes = fromFragmentType.GetCustomAttributes(typeof(MvxFragmentAttribute), true);
            
            if (!attributes.Any())
                throw new InvalidOperationException($"Type does not have {nameof(MvxFragmentAttribute)} attribute!");

            var cacheableFragmentAttribute = attributes.First() as MvxFragmentAttribute;
            return cacheableFragmentAttribute;
        }

        public static bool IsFragmentCacheable(this Type fragmentType)
        {
            if (!fragmentType.HasMvxFragmentAttribute())
             return false;

            var mvxFragmentAttribute = fragmentType.GetMvxFragmentAttribute();
            return mvxFragmentAttribute.IsCacheableFragment;
        }

		public static Type GetViewModelType(this Type fragmentType)
		{
			if (!fragmentType.HasMvxFragmentAttribute())
				return null;

			var mvxFragmentAttribute = fragmentType.GetMvxFragmentAttribute();
			return mvxFragmentAttribute.ViewModelType;
		}
    }
}