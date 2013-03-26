// MvxConventionAttributeExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.CrossCore.Platform
{
    public static class MvxConventionAttributeExtensionMethods
    {
        public static bool IsConventionalByAttribute(this Type candidateType)
        {
            var unconventionalAttributes = candidateType.GetCustomAttributes(typeof (MvxUnconventionalAttribute),
                                                                             true);
            if (unconventionalAttributes.Length > 0)
                return false;

            return candidateType.CheckConditionalAttribributes();
        }

        private static bool CheckConditionalAttribributes(this Type candidateType)
        {
            var conditionalAttributes =
                candidateType.GetCustomAttributes(typeof (MvxConditionalConventionalAttribute), true);

            foreach (MvxConditionalConventionalAttribute conditional in conditionalAttributes)
            {
                var result = conditional.IsConventional;
                if (!result)
                    return false;
            }
            return true;
        }
    }
}