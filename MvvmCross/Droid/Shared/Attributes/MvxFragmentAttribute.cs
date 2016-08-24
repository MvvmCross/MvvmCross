// MvxUnconventionalAttribute.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Droid.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MvxFragmentAttribute : Attribute
    {
        public MvxFragmentAttribute(Type parentActivityViewModelType, int fragmentContentId, bool addToBackStack = false)
        {
            ParentActivityViewModelType = parentActivityViewModelType;
            FragmentContentId = fragmentContentId;
            AddToBackStack = addToBackStack;
        }

        /// <summary>
		/// That shall be used only if you are using non generic fragments.
		/// </summary>
		public Type ViewModelType { get; set; }

        /// <summary>
        /// Indicates if the fragment can be cached. True by default.
        /// </summary>
        public bool IsCacheableFragment { get; set; } = true;

        /// <summary>
        /// Fragment parent activity ViewModel Type. This activity is shown if ShowToViewModel call for Fragment is called from other activity.
        /// </summary>
        public Type ParentActivityViewModelType { get; private set; }

        /// <summary>
        /// Content id - place where to show fragment.
        /// </summary>
        public int FragmentContentId { get; private set; }

        /// <summary>
        /// Indicates if the fragment can be cached. True by default.
        /// </summary>
        public bool AddToBackStack { get; set; } = false;
    }
}