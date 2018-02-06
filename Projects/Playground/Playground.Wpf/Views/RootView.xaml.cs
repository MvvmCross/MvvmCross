using MvvmCross.Core.ViewModels;
using Playground.Core.ViewModels;

namespace Playground.Wpf.Views
{

    [MvxViewFor(typeof(RootViewModel))]
    public partial class RootView 
    {
        public RootView()
        {
            InitializeComponent();
        }
    }
}
