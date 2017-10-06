using System;
using MvvmCross.Core.Views;

namespace MvvmCross.Forms.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MvxContentPagePresentationAttribute : MvxPagePresentationAttribute
    {
        public MvxContentPagePresentationAttribute()
        {
        }

        public bool Animated { get; set; } = true;
    }
}
