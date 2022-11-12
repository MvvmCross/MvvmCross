// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using AppKit;
using MvvmCross.Presenters.Attributes;

namespace MvvmCross.Platforms.Mac.Presenters.Attributes
{
    public class MvxWindowPresentationAttribute : MvxBasePresentationAttribute
    {
        public static float DefaultPositionX = 200;
        public static float DefaultPositionY = 200;
        public static float DefaultWidth = 600;
        public static float DefaultHeight = 400;
        public static NSWindowStyle DefaultWindowStyle = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;
        public static NSBackingStore DefaultBufferingType = NSBackingStore.Buffered;
        public static NSWindowTitleVisibility DefaultTitleVisibility = NSWindowTitleVisibility.Visible;
        public static bool DefaultShouldCascadeWindows = true;

        public MvxWindowPresentationAttribute(string windowControllerName = null, string storyboardName = null)
        {
            WindowControllerName = windowControllerName;
            StoryboardName = storyboardName;
        }

        public float PositionX { get; set; } = DefaultPositionX;

        public float PositionY { get; set; } = DefaultPositionY;

        public float Width { get; set; } = DefaultWidth;

        public float Height { get; set; } = DefaultHeight;

        public NSWindowStyle WindowStyle { get; set; } = DefaultWindowStyle;

        public NSBackingStore BufferingType { get; set; } = DefaultBufferingType;

        public NSWindowTitleVisibility TitleVisibility { get; set; } = DefaultTitleVisibility;

        public bool ShouldCascadeWindows { get; set; } = DefaultShouldCascadeWindows;

        public string Identifier { get; set; }

        public string WindowControllerName { get; set; }

        public string StoryboardName { get; set; }
    }
}
