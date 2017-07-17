using System;
using AppKit;

namespace MvvmCross.Mac.Views.Presenters.Attributes
{
    public class MvxWindowPresentationAttribute : MvxBasePresentationAttribute
    {
        public float PositionX { get; set; } = 200;

        public float PositionY { get; set; } = 200;

        public float Width { get; set; } = 600;

        public float Height { get; set; } = 400;

        public NSWindowStyle WindowStyle { get; set; } = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

        public NSBackingStore BufferingType { get; set; } = NSBackingStore.Buffered;

        public string Identifier { get; set; }
    }
}
