// MvxConventionAttributeExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;

namespace MvvmCross.Droid.FullFragging
{
    public static class MvxFragmentAttributeExtensionMethods
    {
        public static bool IsCacheableFragmentAttribute(this Type candidateType)
        {
            var attributes = candidateType.GetCustomAttributes(typeof(MvxFragmentAttribute), true);
            return attributes.Length > 0;
        }

        public static MvxFragmentAttribute GetCacheableFragmentAttribute(this Type fromFragmentType)
        {
            var attributes = fromFragmentType.GetCustomAttributes(typeof (MvxFragmentAttribute), true);

            if (!attributes.Any())
                throw new InvalidOperationException($"Type does not have {nameof(MvxFragmentAttribute)} attribute!");

            var cacheableFragmentAttribute = attributes.First() as MvxFragmentAttribute;
            return cacheableFragmentAttribute;
        }
    }
}