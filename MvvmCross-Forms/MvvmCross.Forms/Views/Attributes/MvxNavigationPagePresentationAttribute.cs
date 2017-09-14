using System;
using MvvmCross.Core.Views;

namespace MvvmCross.Forms.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MvxNavigationPagePresentationAttribute : MvxBasePresentationAttribute
    {
        public MvxNavigationPagePresentationAttribute()
        {
        }
    }
}