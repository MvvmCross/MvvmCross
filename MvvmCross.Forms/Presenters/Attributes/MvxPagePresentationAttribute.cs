// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Presenters;

namespace MvvmCross.Forms.Presenters.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class MvxPagePresentationAttribute : MvxBasePresentationAttribute
    {
        public MvxPagePresentationAttribute()
        {
        }

        /// <summary>
        /// ViewModel Type to show as host before showing the actual view. Optional when not using switching between Forms views and native views.
        /// </summary>
        public Type HostViewModelType { get; set; }

        /// <summary>
        /// Wraps the Page in a MvxNavigationPage if set to true. If the current stack already is a MvxNavigationPage it will push the Page onto that.
        /// </summary>
        /// <value><c>true</c> if wrap in navigation page; otherwise, <c>false</c>.</value>
        public virtual bool WrapInNavigationPage { get; set; } = true;

        /// <summary>
        /// Clears the backstack of the current NavigationPage when set to true
        /// </summary>
        /// <value><c>true</c> if no history; otherwise, <c>false</c>.</value>
        public virtual bool NoHistory { get; set; } = false;

        public bool Animated { get; set; } = true;

        public string Title { get; set; }

        public string Icon { get; set; }
    }
}
