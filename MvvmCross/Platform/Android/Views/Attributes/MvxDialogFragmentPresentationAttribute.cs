using System;

namespace MvvmCross.Droid.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MvxDialogFragmentPresentationAttribute : MvxFragmentPresentationAttribute
    {
        public MvxDialogFragmentPresentationAttribute()
        {
        }

        public MvxDialogFragmentPresentationAttribute(bool cancelable = true, Type activityHostViewModelType = null,
            bool addToBackStack = false, int enterAnimation = int.MinValue, int exitAnimation = int.MinValue,
            int popEnterAnimation = int.MinValue, int popExitAnimation = int.MinValue,
            int transitionStyle = int.MinValue, bool isCacheableFragment = false) : base(activityHostViewModelType,
            int.MinValue, addToBackStack, enterAnimation, exitAnimation, popEnterAnimation, popExitAnimation,
            transitionStyle, null, isCacheableFragment)
        {
            Cancelable = cancelable;
        }

        public MvxDialogFragmentPresentationAttribute(bool cancelable = true, Type activityHostViewModelType = null,
            bool addToBackStack = false, string enterAnimation = null, string exitAnimation = null,
            string popEnterAnimation = null, string popExitAnimation = null, string transitionStyle = null,
            bool isCacheableFragment = false) : base(activityHostViewModelType, null, addToBackStack, enterAnimation,
            exitAnimation, popEnterAnimation, popExitAnimation, transitionStyle, null, isCacheableFragment)
        {
            Cancelable = cancelable;
        }

        public static bool DefaultCancelable = true;
        /// <summary>
        ///     Indicates if the dialog can be canceled
        /// </summary>
        public bool Cancelable { get; set; } = DefaultCancelable;
    }
}