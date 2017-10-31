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

        public virtual bool WrapInNavigationPage { get; set; } = false;

        /// <summary>
        /// Clears the backstack of the current NavigationPage when set to true
        /// </summary>
        /// <value><c>true</c> if no history; otherwise, <c>false</c>.</value>
        public virtual bool NoHistory { get; set; } = false;

        public bool Animated { get; set; } = true;
    }
}