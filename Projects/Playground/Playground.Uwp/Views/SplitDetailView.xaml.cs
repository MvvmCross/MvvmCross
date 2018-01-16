using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MvvmCross.Uwp.Attributes;
using MvvmCross.Uwp.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Playground.Uwp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [MvxSplitViewPresentation(Position = SplitPanePosition.Content)]
    public sealed partial class SplitDetailView : MvxWindowsPage
    {
        public SplitDetailView()
        {
            this.InitializeComponent();
        }
    }
}
