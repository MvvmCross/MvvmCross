// MvxUnconventionalAttribute.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using System;
using System.Collections.Concurrent;

namespace MvvmCross.Droid.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MvxFragmentAttribute : Attribute
    {

        private static readonly ConcurrentDictionary<string, int> FragmentNameMappings = new ConcurrentDictionary<string, int>();
        public MvxFragmentAttribute(Type parentActivityViewModelType, int fragmentContentId, bool addToBackStack = false)
        {
            ParentActivityViewModelType = parentActivityViewModelType;
            FragmentContentId = fragmentContentId;
            AddToBackStack = addToBackStack;
        }

        public MvxFragmentAttribute(Type parentActivityViewModelType, string fragmentContentName, bool addToBackStack = false)
        {
            ParentActivityViewModelType = parentActivityViewModelType;
            FragmentContentName = fragmentContentName;
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
        public int? FragmentContentId { get; private set; }

        /// <summary>
        /// Content name - place where to show fragment.
        /// </summary>
        public string FragmentContentName { get; private set; }

        /// <summary>
        /// Indicates if the fragment can be cached. False by default.
        /// </summary>
        public bool AddToBackStack { get; set; } = false;

        /// <summary>
        /// Determines the content id according to whether the name or id has been specified.
        /// </summary>
        /// <param name="activity">The activity to use to retrieve the id</param>
        /// <returns>The content id</returns>
        public int GetFragmentContentId(Activity activity)
        {
            int contentId;
            if (this.FragmentContentId != null)
            {
                contentId = this.FragmentContentId.Value;
            }
            else
            {
                if (!FragmentNameMappings.TryGetValue(this.FragmentContentName, out contentId))
                {
                    contentId = activity.Resources.GetIdentifier(this.FragmentContentName, "id", activity.PackageName);
                    FragmentNameMappings.TryAdd(this.FragmentContentName, contentId);
                }
            }
            return contentId;
        }
    }
}