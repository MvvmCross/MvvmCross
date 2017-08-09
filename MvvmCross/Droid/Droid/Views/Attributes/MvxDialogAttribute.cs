using System;
using System.Collections.Generic;
using Android.Views;
using MvvmCross.Core.Views;

namespace MvvmCross.Droid.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MvxDialogAttribute : MvxBasePresentationAttribute
    {
        public bool Cancelable { get; set; } = true;

        /// <summary>
        /// Indicates if the fragment can be cached. False by default.
        /// </summary>
        public bool AddToBackStack { get; set; } = false;

        public int EnterAnimation { get; set; } = int.MinValue;

        public int ExitAnimation { get; set; } = int.MinValue;

        public int PopEnterAnimation { get; set; } = int.MinValue;

        public int PopExitAnimation { get; set; } = int.MinValue;

        public int TransitionStyle { get; set; } = int.MinValue;

        /// <summary>
        /// SharedElements that will be added to the transition. String may be left empty when using AppCompat
        /// </summary>
        public IDictionary<string, View> SharedElements { get; set; }
    }
}