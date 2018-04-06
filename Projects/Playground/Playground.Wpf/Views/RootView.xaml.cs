using System.Windows;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;

namespace Playground.Wpf.Views
{

    [MvxViewFor(typeof(RootViewModel))]
    public partial class RootView
    {
        public new RootViewModel ViewModel
        {
            get { return (RootViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        public RootView()
        {
            InitializeComponent();
            Loaded += MainView_Loaded;
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowMenu();
        }
    }
}
