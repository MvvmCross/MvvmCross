using System;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.AutoView.Touch.Views
{
    public class MvxMissingViewFinder : IMvxViewFinder
    {
        public Type MissingViewType { get; set; }

        public MvxMissingViewFinder()
        {
            MissingViewType = typeof (MvxMissingViewController);
        }

        public Type GetViewType(Type viewModelType)
        {
            return MissingViewType;
        }
    }
}