// MvxConventionAttributeExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Droid.FullFragging
{
    public static class MvxOwnedViewModelFragmentAttributeExtensionMethods
    {
        public static bool IsOwnedViewModelFragment(this Type candidateType)
        {
            var attributes = candidateType.GetCustomAttributes(typeof(MvxOwnedViewModelFragmentAttribute), true);
            return attributes.Length > 0;
        }
    }
}