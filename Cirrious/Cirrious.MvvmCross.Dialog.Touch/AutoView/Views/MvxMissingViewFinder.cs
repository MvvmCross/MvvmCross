using System;
using Cirrious.MvvmCross.AutoView.Droid.Views.Lists;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.AutoView.Droid.Views
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