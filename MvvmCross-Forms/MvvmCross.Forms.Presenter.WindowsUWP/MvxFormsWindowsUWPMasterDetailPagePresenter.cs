using MvvmCross.WindowsUWP.Views;
using MvvmCross.Forms.Presenter.Core;
using Xamarin.Forms;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Forms.Presenter.WindowsUWP
{
    public class MvxFormsWindowsUWPMasterDetailPagePresenter
        : MvxFormsMasterDetailPagePresenter
        , IMvxWindowsViewPresenter
    {
        private readonly IMvxWindowsFrame _rootFrame;        

        public MvxFormsWindowsUWPMasterDetailPagePresenter(IMvxWindowsFrame rootFrame, Application mvxFormsApp)
            : base(mvxFormsApp)
        {
            _rootFrame = rootFrame;
        }

        protected override void CustomPlatformInitialization(MasterDetailPage mainPage)
        {
            _rootFrame.Navigate(mainPage.GetType(), _rootFrame);            
        }      
    }
}