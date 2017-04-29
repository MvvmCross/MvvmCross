// MvxFormsWindowsPhonePagePresenter.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// MvvmCross.Forms.Presenter is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com


using MvvmCross.Forms.Presenters;
using MvvmCross.WindowsUWP.Views;
using Xamarin.Forms;

namespace MvvmCross.Forms.Uwp.Presenters
{
    public class MvxFormsWindowsUWPPagePresenter
        : MvxFormsPagePresenter
        , IMvxWindowsViewPresenter
    {
        private readonly IMvxWindowsFrame _rootFrame;

        public MvxFormsWindowsUWPPagePresenter(IMvxWindowsFrame rootFrame, Application mvxFormsApp)
            : base(mvxFormsApp)
        {
            _rootFrame = rootFrame;
        }

        protected override void CustomPlatformInitialization(NavigationPage mainPage)
        {
            _rootFrame.Navigate(mainPage.GetType(), _rootFrame);
        }
    }
}