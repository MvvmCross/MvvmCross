using System;
using MvvmCross.Core.Views;
using System.Windows;

namespace MvvmCross.Wpf.Views.Presenters.Attributes
{
    public class MvxWindowPresentationAttribute : MvxBasePresentationAttribute
    {
        public string Identifier { get; set; }
        public bool Modal { get; set; }
    }
}
