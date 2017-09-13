using System;
using AppKit;
using MvvmCross.Core.Views;

namespace MvvmCross.Mac.Views.Presenters.Attributes
{
    public class MvxStoryboardWindowPresentationAttribute : MvxBasePresentationAttribute
    {
        public MvxStoryboardWindowPresentationAttribute(string windowControllerName, string storyboardName)
        {
            this.WindowControllerName = windowControllerName;
            this.StoryboardName = storyboardName;
        }

        public string StoryboardName { get; set; }

        public string WindowControllerName { get; set; }

        public string Identifier { get; set; }
    }
}
