// MvxUnconventionalAttribute.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Droid.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MvxFragmentAttribute : MvxBasePresentationAttribute
    {
        public MvxFragmentAttribute(Type parentActivityViewModelType, int fragmentContentId, bool addToBackStack = false)
        {
            ParentActivityViewModelType = parentActivityViewModelType;
            FragmentContentId = fragmentContentId;
            AddToBackStack = addToBackStack;
        }

        /// <summary>
        /// Indicates if the fragment can be cached. True by default.
        /// </summary>
        public bool IsCacheableFragment { get; set; } = true;

        /// <summary>
        /// Content id - place where to show fragment.
        /// </summary>
        public int FragmentContentId { get; private set; }

        /// <summary>
        /// Indicates if the fragment can be cached. False by default.
        /// </summary>
        public bool AddToBackStack { get; set; } = false;
    }
}