using MvvmCross.Forms.Platform;
using MvvmCross.Uwp.Views;
using Xamarin.Forms;

/*
namespace MvvmCross.Forms.Uwp.Presenters
{
    public class MvxFormsUwpMasterDetailPagePresenter
        : MvxFormsMasterDetailPagePresenter
        , IMvxWindowsViewPresenter
    {
        private readonly IMvxWindowsFrame _rootFrame;        

        public MvxFormsUwpMasterDetailPagePresenter(IMvxWindowsFrame rootFrame, MvxFormsApplication formsApplication)
            : base(formsApplication)
        {
            _rootFrame = rootFrame;
        }

        protected override void CustomPlatformInitialization(MasterDetailPage mainPage)
        {
            _rootFrame.Navigate(mainPage.GetType(), _rootFrame);            
        }      
    }
}*/