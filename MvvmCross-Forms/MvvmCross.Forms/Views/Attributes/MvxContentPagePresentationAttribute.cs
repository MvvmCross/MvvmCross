using System;
using MvvmCross.Core.Views;

namespace MvvmCross.Forms.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MvxContentPagePresentationAttribute : MvxBasePresentationAttribute
    {
        public MvxContentPagePresentationAttribute()
        {
        }

        public bool WrapInNavigationPage { get; set; } = true;
        public bool Animated { get; set; } = true;
    }
}
