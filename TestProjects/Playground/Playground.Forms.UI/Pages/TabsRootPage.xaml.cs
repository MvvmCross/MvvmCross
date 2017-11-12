using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Forms.Pages
{
    [MvxTabbedPagePresentation(TabbedPosition.Root)]
    public partial class TabsRootPage : MvxTabbedPage<TabsRootViewModel>
    {
        public TabsRootPage()
        {
            InitializeComponent();
        }

        private bool _firstTime = true;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_firstTime)
            {
                //ViewModel.ShowInitialViewModelsCommand.Execute();
                ViewModel.ShowInitialViewModelsCommand.ExecuteAsync(null);
                _firstTime = false;
            }
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
        }
    }
}
