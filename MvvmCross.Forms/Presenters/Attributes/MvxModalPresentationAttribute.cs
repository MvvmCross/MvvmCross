// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Forms.Presenters.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MvxModalPresentationAttribute : MvxPagePresentationAttribute
    {
        public MvxModalPresentationAttribute()
        {
        }

        /// <summary>
        /// set the modal presentation style of the modal. This is used for iOS devices for setting different types of modals.
        /// </summary>
        public MvxFormsModalPresentationStyle PresentationStyle { get; set; }
    }

    public enum MvxFormsModalPresentationStyle
    {
        /// <summary>
        /// A presentation style in which the presented view covers the screen.
        /// </summary>
        FullScreen = 0,

        /// <summary>
        /// A presentation style that displays the content centered in the screen.
        /// </summary>
        FormSheet = 1,

        /// <summary>
        /// The default presentation style chosen by the system.
        /// </summary>
        Automatic = 2,

        /// <summary>
        /// A view presentation style in which the presented view covers the screen.
        /// </summary>
        OverFullScreen = 3,

        /// <summary>
        /// A presentation style that partially covers the underlying content.
        /// </summary>
        PageSheet = 4
    }
}
