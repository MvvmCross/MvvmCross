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

        public ModalPresentationStyle PresentationStyle { get; set; }
    }

    public enum ModalPresentationStyle
    {
        FullScreen = 0,
        FormSheet = 1,
        Automatic = 2,
        OverFullScreen = 3,
        PageSheet = 4
    }
}
