using System;
using MvvmCross.Core.Views;

namespace MvvmCross.Forms.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class MvxPagePresentationAttribute : MvxBasePresentationAttribute
    {
        public MvxPagePresentationAttribute()
        {
        }

        public bool WrapInNavigationPage { get; set; } = true;
        public string Title { get; set; }
        public string Icon { get; set; }
    }
}
