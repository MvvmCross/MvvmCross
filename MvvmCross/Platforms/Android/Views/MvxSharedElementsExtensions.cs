// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Android.OS;
using Android.Views;
using MvvmCross.Platforms.Android.Presenters;

namespace MvvmCross.Platforms.Android.Views
{
    public static class MvxSharedElementsExtensions
    {
        /// <summary>
        /// Gets the identifier and transition name pairs.
        /// </summary>
        /// <param name="bundle">A <see cref="Bundle"/> containing shared element metadata passed to the fragment or activity.</param>
        /// <returns>An <see cref="IDictionary{key, value}"/> containing the identifier key and transition name value.</returns>
        public static IDictionary<string, string> GetSharedElementTransitionNames(this Bundle bundle)
        {
            IDictionary<string, string> sharedElements = new Dictionary<string, string>();
            var transitions = bundle.GetString(MvxAndroidViewPresenter.SharedElementsBundleKey);

            foreach (var transition in transitions.Split('|'))
            {
                string[] transitionDetails = transition.Split(':');
                sharedElements.Add(new KeyValuePair<string, string>(transitionDetails[0], transitionDetails[1]));
            }

            return sharedElements;
        }

        /// <summary>
        /// Sets the corresponding views transition name, based on view tags.
        /// </summary>
        /// <param name="bundle">A <see cref="Bundle"/> containing shared element metadata passed to the fragment or activity.</param>
        /// <param name="view">The <see cref="View"/> to search for the tag.</param>
        public static void SetSharedElementsByTag(this Bundle bundle, View view)
        {
            var transitions = bundle.GetString(MvxAndroidViewPresenter.SharedElementsBundleKey);

            foreach (var transition in transitions.Split('|'))
            {
                string[] transitionDetails = transition.Split(':');
                View viewToAnimate = view.FindViewWithTag(transitionDetails[0]);
                viewToAnimate.SetTransitionNameSupport(transitionDetails[1]);
            }
        }

        /// <summary>
        /// Sets the corresponding views transition name, based on view identifier.
        /// </summary>
        /// <param name="bundle">A <see cref="Bundle"/> containing shared element metadata passed to the fragment or activity.</param>
        /// <param name="view">The <see cref="View"/> to search for the identifier.</param>
        public static void SetSharedElementsById(this Bundle bundle, View view)
        {
            var transitions = bundle.GetString(MvxAndroidViewPresenter.SharedElementsBundleKey);

            foreach (var transition in transitions.Split('|'))
            {
                string[] transitionDetails = transition.Split(':');
                View viewToAnimate = view.FindViewById(view.Context.Resources.GetIdentifier(transitionDetails[0], "id", view.Context.PackageName));
                viewToAnimate.SetTransitionNameSupport(transitionDetails[1]);
            }
        }

        /// <summary>
        /// Sets the <see cref="View.TransitionName"/> with platform support checks
        /// </summary>
        /// <param name="view">The <see cref="View"/> to use.</param>
        /// <param name="transitionName">The name for the transition.</param>
        public static void SetTransitionNameSupport(this View view, string transitionName)
        {
            view.TransitionName = transitionName;
        }

        /// <summary>
        /// Gets the <see cref="View.TransitionName"/> with platform support checks
        /// </summary>
        /// <param name="view">The <see cref="View"/> to use.</param>
        /// <returns>The name for the transition.</returns>
        public static string GetTransitionNameSupport(this View view)
        {
            return view.TransitionName;
        }
    }
}
