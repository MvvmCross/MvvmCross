// MvxFormsTouchPagePresenter.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// Cirrious.MvvmCross.Forms.Presenter is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com

using Cirrious.MvvmCross.Forms.Presenter.Core;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using UIKit;
using Xamarin.Forms;

namespace Cirrious.MvvmCross.Forms.Presenter.Touch
{
    public class MvxFormsTouchPagePresenter
        : MvxFormsPagePresenter
        , IMvxTouchViewPresenter
    {
        private readonly UIWindow _window;

        public MvxFormsTouchPagePresenter(UIWindow window, Application mvxFormsApp)
            : base(mvxFormsApp)
        {
            _window = window;
        }

        public virtual bool PresentModalViewController(UIViewController controller, bool animated)
        {
            return false;
        }

        public virtual void NativeModalViewControllerDisappearedOnItsOwn()
        {
        }

        protected override void CustomPlatformInitialization(NavigationPage mainPage)
        {
            _window.RootViewController = mainPage.CreateViewController();
        }
    }
}