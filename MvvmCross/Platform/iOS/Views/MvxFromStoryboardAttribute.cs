// MvxFromStoryboardAttribute.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.iOS.Views
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MvxFromStoryboardAttribute : Attribute
    {
        public string StoryboardName { get; set; }

        public MvxFromStoryboardAttribute(string storyboardName = null)
        {
            StoryboardName = storyboardName;
        }
    }
}