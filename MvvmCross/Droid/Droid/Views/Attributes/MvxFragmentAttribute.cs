using System;
using System.Collections.Generic;
using Android.Views;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid;

namespace MvvmCross.Droid.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MvxFragmentAttribute : MvxBasePresentationAttribute
    {
        public MvxFragmentAttribute()
        {
        }

        public MvxFragmentAttribute(
            Type activityHostViewModelType = null, 
            int fragmentContentId = Android.Resource.Id.Content, 
            bool addToBackStack = false,
            int enterAnimation = int.MinValue,
            int exitAnimation = int.MinValue,
            int popEnterAnimation = int.MinValue,
            int popExitAnimation = int.MinValue,
            int transitionStyle = int.MinValue,
            bool isCacheableFragment = true
        )
        {
            ActivityHostViewModelType = activityHostViewModelType;
            FragmentContentId = fragmentContentId;
            AddToBackStack = addToBackStack;
            EnterAnimation = enterAnimation;
            ExitAnimation = exitAnimation;
            PopEnterAnimation = popEnterAnimation;
            PopExitAnimation = popExitAnimation;
            TransitionStyle = transitionStyle;
            IsCacheableFragment = isCacheableFragment;
        }

        public MvxFragmentAttribute(
            Type activityHostViewModelType, 
            string fragmentContentResourceName, 
            bool addToBackStack = false,
            string enterAnimation = null,
            string exitAnimation = null,
            string popEnterAnimation = null,
            string popExitAnimation = null,
            string transitionStyle = null,
            bool isCacheableFragment = true
        )
        {
            var context = Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext;

            ActivityHostViewModelType = activityHostViewModelType;
            FragmentContentId = fragmentContentResourceName!=null ? context.Resources.GetIdentifier(fragmentContentResourceName, "id", context.PackageName) : Android.Resource.Id.Content;
            AddToBackStack = addToBackStack;
            EnterAnimation = enterAnimation!=null ? context.Resources.GetIdentifier(enterAnimation, "animation", context.PackageName) : int.MinValue;
            ExitAnimation = exitAnimation!= null ? context.Resources.GetIdentifier(exitAnimation, "animation", context.PackageName) : int.MinValue;
            PopEnterAnimation = popEnterAnimation!= null ? context.Resources.GetIdentifier(popEnterAnimation, "animation", context.PackageName) : int.MinValue;
            PopExitAnimation = popExitAnimation!= null ? context.Resources.GetIdentifier(popExitAnimation, "animation", context.PackageName) : int.MinValue;
            TransitionStyle = transitionStyle!= null ? context.Resources.GetIdentifier(transitionStyle, "style", context.PackageName) : int.MinValue;
            IsCacheableFragment = isCacheableFragment;
        }

        /// <summary>
        /// Fragment parent activity ViewModel Type. This activity is shown if ShowToViewModel call for Fragment is called from other activity.
        /// </summary>
        public Type ActivityHostViewModelType { get; set; }

        /// <summary>
        /// Content id - place where to show fragment.
        /// </summary>
        public int FragmentContentId { get; set; }

        /// <summary>
        /// Indicates if the fragment can be cached. False by default.
        /// </summary>
        public bool AddToBackStack { get; set; } = false;

        public int EnterAnimation { get; set; } = int.MinValue;

        public int ExitAnimation { get; set; } = int.MinValue;

        public int PopEnterAnimation { get; set; } = int.MinValue;

        public int PopExitAnimation { get; set; } = int.MinValue;

        public int TransitionStyle { get; set; } = int.MinValue;

        /// <summary>
        /// Indicates if the fragment can be cached. True by default.
        /// </summary>
        public bool IsCacheableFragment { get; set; } = true;

        /// <summary>
        /// SharedElements that will be added to the transition. String may be left empty when using AppCompat
        /// </summary>
        public IDictionary<string, View> SharedElements { get; set; }
    }
}