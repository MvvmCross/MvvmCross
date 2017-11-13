using System;
using MvvmCross.Core.Views;

namespace MvvmCross.Mac.Views.Presenters.Attributes
{
    public class MvxTabPresentationAttribute : MvxBasePresentationAttribute
    {
        public string WindowIdentifier { get; set; }

        public string TabTitle { get; set; }
    }
}
