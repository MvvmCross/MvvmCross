// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Presenters.Attributes;

namespace MvvmCross.Platforms.Android.Presenters.Attributes
{
#nullable enable
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MvxFragmentPresentationAttribute : MvxBasePresentationAttribute
    {
        public MvxFragmentPresentationAttribute()
        {
        }

        public MvxFragmentPresentationAttribute(
            Type? activityHostViewModelType = null,
            int fragmentContentId = global::Android.Resource.Id.Content,
            bool addToBackStack = false,
            int enterAnimation = int.MinValue,
            int exitAnimation = int.MinValue,
            int popEnterAnimation = int.MinValue,
            int popExitAnimation = int.MinValue,
            int transitionStyle = int.MinValue,
            Type? fragmentHostViewType = null,
            bool isCacheableFragment = false,
            string? tag = null,
            string popBackStackImmediateName = "",
            MvxPopBackStack popBackStackImmediateFlag = MvxPopBackStack.Inclusive,
            bool addFragment = false
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
            FragmentHostViewType = fragmentHostViewType;
            IsCacheableFragment = isCacheableFragment;
            Tag = tag;
            PopBackStackImmediateName = popBackStackImmediateName;
            PopBackStackImmediateFlag = popBackStackImmediateFlag;
            AddFragment = addFragment;
        }

        public MvxFragmentPresentationAttribute(
            Type? activityHostViewModelType = null,
            string? fragmentContentResourceName = null,
            bool addToBackStack = false,
            string? enterAnimation = null,
            string? exitAnimation = null,
            string? popEnterAnimation = null,
            string? popExitAnimation = null,
            string? transitionStyle = null,
            Type? fragmentHostViewType = null,
            bool isCacheableFragment = false,
            string? tag = null,
            string popBackStackImmediateName = "",
            MvxPopBackStack popBackStackImmediateFlag = MvxPopBackStack.Inclusive,
            bool addFragment = false
        )
        {
            if (Mvx.IoCProvider.TryResolve(out IMvxAndroidGlobals globals) &&
                globals.ApplicationContext.Resources != null)
            {
                var context = globals.ApplicationContext;

                FragmentContentId = !string.IsNullOrEmpty(fragmentContentResourceName) ?
                    context.Resources.GetIdentifier(fragmentContentResourceName, "id", context.PackageName) :
                    global::Android.Resource.Id.Content;

                EnterAnimation = !string.IsNullOrEmpty(enterAnimation) ?
                    context.Resources.GetIdentifier(enterAnimation, "animation", context.PackageName) :
                    int.MinValue;

                ExitAnimation = !string.IsNullOrEmpty(exitAnimation) ?
                    context.Resources.GetIdentifier(exitAnimation, "animation", context.PackageName) :
                    int.MinValue;

                PopEnterAnimation = !string.IsNullOrEmpty(popEnterAnimation) ?
                    context.Resources.GetIdentifier(popEnterAnimation, "animation", context.PackageName) :
                    int.MinValue;

                PopExitAnimation = !string.IsNullOrEmpty(popExitAnimation) ?
                    context.Resources.GetIdentifier(popExitAnimation, "animation", context.PackageName) :
                    int.MinValue;

                TransitionStyle = !string.IsNullOrEmpty(transitionStyle) ?
                    context.Resources.GetIdentifier(transitionStyle, "style", context.PackageName) :
                    int.MinValue;
            }

            ActivityHostViewModelType = activityHostViewModelType;
            AddToBackStack = addToBackStack;
            FragmentHostViewType = fragmentHostViewType;
            IsCacheableFragment = isCacheableFragment;
            Tag = tag;
            PopBackStackImmediateName = popBackStackImmediateName;
            PopBackStackImmediateFlag = popBackStackImmediateFlag;
            AddFragment = addFragment;
        }

        /// <summary>
        /// Fragment parent activity ViewModel Type. This activity is shown if the current hosting activity viewmodel is different.
        /// </summary>
        public Type? ActivityHostViewModelType { get; set; }

        /// <summary>
        /// Fragment parent View Type. When set ChildFragmentManager of this Fragment will be used
        /// </summary>
        public Type? FragmentHostViewType { get; set; }

        /// <summary>
        /// Content id - place where to show fragment.
        /// </summary>
        public int FragmentContentId { get; set; }

        public static bool DefaultAddToBackStack { get; }
        /// <summary>
        /// Will add the Fragment to the FragmentManager backstack
        /// </summary>
        public bool AddToBackStack { get; set; }

        public static int DefaultEnterAnimation { get; } = int.MinValue;
        /// <summary>
        /// Animation when Fragment is shown
        /// </summary>
        public int EnterAnimation { get; set; } = DefaultEnterAnimation;

        public static int DefaultExitAnimation { get; } = int.MinValue;
        /// <summary>
        /// Animation when Fragment is closed
        /// </summary>
        public int ExitAnimation { get; set; } = DefaultExitAnimation;

        public static int DefaultPopEnterAnimation { get; } = int.MinValue;
        public int PopEnterAnimation { get; set; } = DefaultPopEnterAnimation;

        public static int DefaultPopExitAnimation { get; } = int.MinValue;
        public int PopExitAnimation { get; set; } = DefaultPopExitAnimation;

        public static int DefaultTransitionStyle { get; } = int.MinValue;
        /// <summary>
        /// TransitionStyle for Fragment
        /// </summary>
        public int TransitionStyle { get; set; } = DefaultTransitionStyle;

        public static bool DefaultIsCacheableFragment { get; }

        /// <summary>
        /// Indicates if the fragment can be cached. False by default.
        /// </summary>
        public bool IsCacheableFragment { get; set; } = DefaultIsCacheableFragment;

        /// <summary>
        /// Tag for the Fragment. Used in transactions and for finding the Fragment at a later time
        /// </summary>
        public string? Tag { get; set; }

        public static string DefaultPopBackStackImmediateName { get; } = string.Empty;

        /// <summary>
        /// The name to be passed into PopBackStackImmediate.
        /// Assigning an empty string will default to using the FragmentJavaName
        /// Assigning a null will pop the top fragment
        /// </summary>
        public string PopBackStackImmediateName { get; set; } = DefaultPopBackStackImmediateName;

        public static MvxPopBackStack DefaultPopBackStackImmediateFlag { get; } = MvxPopBackStack.Inclusive;

        /// <summary>
        /// Flag to be used with PopBackStackImmediate. 
        /// </summary>
        public MvxPopBackStack PopBackStackImmediateFlag { get; set; } = DefaultPopBackStackImmediateFlag;

        /// <summary>
        /// Setting this to true, will use Add instead of Replace on the Fragment transaction
        /// </summary>
        public bool AddFragment { get; set; }
    }
#nullable restore
}
