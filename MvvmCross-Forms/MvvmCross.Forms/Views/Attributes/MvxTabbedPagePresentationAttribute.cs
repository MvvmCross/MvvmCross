using System;
using MvvmCross.Core.Views;

namespace MvvmCross.Forms.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MvxTabbedPagePresentationAttribute : MvxPagePresentationAttribute
    {
        public MvxTabbedPagePresentationAttribute(TabbedPosition position = TabbedPosition.Tab)
        {
            Position = position;
        }

        public TabbedPosition Position { get; set; }
    }

    public enum TabbedPosition
    {
        Root,
        Tab
    }
}
