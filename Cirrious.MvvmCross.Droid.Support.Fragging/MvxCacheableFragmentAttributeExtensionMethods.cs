// MvxConventionAttributeExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;

namespace Cirrious.MvvmCross.Droid.Support.Fragging
{
    public static class MvxCacheableFragmentAttributeExtensionMethods
    {
        public static bool IsCacheableFragmentAttribute(this Type candidateType)
        {
            var attributes = candidateType.GetCustomAttributes(typeof(MvxCacheableFragmentAttribute), true);
            return attributes.Length > 0;
        }

        public static MvxCacheableFragmentAttribute GetCacheableFragmentAttribute(this Type fromFragmentType)
        {
            var attributes = fromFragmentType.GetCustomAttributes(typeof(MvxCacheableFragmentAttribute), true);
            
            if (!attributes.Any())
                throw new InvalidOperationException($"Type does not have {nameof(MvxCacheableFragmentAttribute)} attribute!");

            var cacheableFragmentAttribute = attributes.First() as MvxCacheableFragmentAttribute;
            return cacheableFragmentAttribute;
        }
    }
}