using System;

namespace MvvmCross.Droid.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MvxDialogAttribute : MvxBasePresentationAttribute
    {
        public bool Cancelable { get; set; } = true;
    }
}