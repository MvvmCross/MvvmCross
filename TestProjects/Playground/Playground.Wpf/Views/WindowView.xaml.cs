using System;
using MvvmCross.Core.Views;
using MvvmCross.Wpf.Views;
using MvvmCross.Wpf.Views.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Wpf.Views
{
    /// <summary>
    /// Interaction logic for WindowView.xaml
    /// </summary>
    public partial class WindowView : MvxWindow<WindowViewModel>, IMvxOverridePresentationAttribute
    {
        public WindowView()
        {
            InitializeComponent();
        }

        public MvxBasePresentationAttribute PresentationAttribute()
        {
            return new MvxWindowPresentationAttribute
            {
                Identifier = $"{nameof(WindowView)}"
            };
        }
    }
}
