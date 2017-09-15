using System;
using AppKit;
using MvvmCross.Core.Views;

namespace MvvmCross.Mac.Views.Presenters.Attributes
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

        public static float InitialPositionX = float.MinValue;
        public static float InitialPositionY = float.MinValue;
        public static float InitialWidth = float.MinValue;
        public static float InitialHeight = float.MinValue;
        public static NSWindowStyle? InitialWindowStyle = null;
        public static NSBackingStore? InitialBufferingType = null;
        public static NSWindowTitleVisibility? InitialTitleVisibilty = null;
        public static bool? InitialShouldCascadeWindows = null;

        public MvxWindowPresentationAttribute(string windowControllerName = null, string storyboardName = null)
        {
            WindowControllerName = windowControllerName;
            StoryboardName = storyboardName;
        }

        public float PositionX { get; set; } = InitialPositionX;

        public float PositionY { get; set; } = InitialPositionY;

        public float Width { get; set; } = InitialWidth;

        public float Height { get; set; } = InitialHeight;

        public NSWindowStyle? WindowStyle { get; set; } = InitialWindowStyle;

        public NSBackingStore? BufferingType { get; set; } = InitialBufferingType;

        public NSWindowTitleVisibility? TitleVisibility { get; set; } = InitialTitleVisibilty;

        public bool? ShouldCascadeWindows { get; set; } = InitialShouldCascadeWindows;

        public string Identifier { get; set; }

        public string WindowControllerName { get; set; }

        public string StoryboardName { get; set; }
    }
}
