// MvxConventionAttributeExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.CrossCore.IoC
{
    public static class MvxConventionAttributeExtensionMethods
    {
        /// <summary>
        /// A type is conventional if and only it is:
        /// - not marked with an unconventional attribute
        /// - all marked conditional conventions return true
        /// </summary>
        /// <param name="candidateType"></param>
        /// <returns></returns>
        public static bool IsConventional(this Type candidateType)
        {
            var unconventionalAttributes = candidateType.GetCustomAttributes(typeof (MvxUnconventionalAttribute),
                                                                             true);
            if (unconventionalAttributes.Length > 0)
                return false;

            return candidateType.SatisfiesConditionalConventions();
        }

        public static bool SatisfiesConditionalConventions(this Type candidateType)
        {
            var conditionalAttributes =
                candidateType.GetCustomAttributes(typeof (MvxConditionalConventionalAttribute), true);

            foreach (MvxConditionalConventionalAttribute conditional in conditionalAttributes)
            {
                var result = conditional.IsConditionSatisfied;
                if (!result)
                    return false;
            }
            return true;
        }
    }
}