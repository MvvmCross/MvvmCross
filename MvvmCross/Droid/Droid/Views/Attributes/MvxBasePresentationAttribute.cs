using System;

namespace MvvmCross.Droid.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class MvxBasePresentationAttribute : Attribute
    {
        /// <summary>
        /// That shall be used only if you are using non generic fragments.
        /// </summary>
        public Type ViewModelType { get; set; }

        /// <summary>
        /// That shall be used only if you are using non generic fragments.
        /// </summary>
        public Type ViewType { get; set; }

        /// <summary>
        /// Fragment parent activity ViewModel Type. This activity is shown if ShowToViewModel call for Fragment is called from other activity.
        /// </summary>
        public Type ParentActivityViewModelType { get; set; }
    }
}
