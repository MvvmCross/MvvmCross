// MvxFormsWindowsPhonePagePresenter.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// Cirrious.MvvmCross.Forms.Presenter is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com

using Cirrious.MvvmCross.Forms.Presenter.Core;
using Cirrious.MvvmCross.WindowsCommon.Views;
using Xamarin.Forms;

namespace Cirrious.MvvmCross.Forms.Presenter.Windows81
{
    public class MvxFormsWindows81PagePresenter
        : MvxFormsPagePresenter
        , IMvxWindowsViewPresenter
    {
        private readonly IMvxWindowsFrame _rootFrame;

        public MvxFormsWindows81PagePresenter(IMvxWindowsFrame rootFrame, Application mvxFormsApp)
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