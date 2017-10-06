using System;
using MvvmCross.Core.Views;

namespace MvvmCross.Forms.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MvxModalPresentationAttribute : MvxPagePresentationAttribute
    {
        public MvxModalPresentationAttribute()
        {
        }
    }
}