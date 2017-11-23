using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Playground.Forms.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Master, WrapInNavigationPage = false, NoHistory = true)]
    public partial class MixedNavMasterDetailPage : MvxContentPage<MixedNavMasterDetailViewModel>
    {
        public MixedNavMasterDetailPage()
        {
            InitializeComponent();

#if __IOS__
            if(Parent is MasterDetailPage master)
                master.IsGestureEnabled = false;
#endif
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            ViewModel.PropertyChanged += (sender, e) => {
                if (e.PropertyName == nameof(ViewModel.SelectedMenu))
                {
                    if (Parent is MasterDetailPage master)
                    {
                        //master.MasterBehavior = MasterBehavior.Popover;
                        master.IsPresented = !master.IsPresented;
                    }
                }
            };
        }
    }
}