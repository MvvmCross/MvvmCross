using MvvmCross.Forms.Presenters;
using MvvmCross.WindowsUWP.Views;
using Xamarin.Forms;

namespace MvvmCross.Forms.Uwp.Presenters
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