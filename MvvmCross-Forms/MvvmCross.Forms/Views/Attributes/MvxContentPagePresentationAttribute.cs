using System;
using MvvmCross.Core.Views;

namespace MvvmCross.Forms.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MvxContentPagePresentationAttribute : MvxBasePresentationAttribute
    {
        public bool WrapInNavigationPage { get; set; }
        public bool Animated { get; set; }
    }
}
