using System;

namespace MvvmCross.Core.Views
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class MvxBasePresentationAttribute : Attribute
    {
        /// <summary>
        /// That shall be used only if you are using non generic views.
        /// </summary>
        public Type ViewModelType { get; set; }

        /// <summary>
        /// Type of the view
        /// </summary>
        public Type ViewType { get; set; }
    }
}
