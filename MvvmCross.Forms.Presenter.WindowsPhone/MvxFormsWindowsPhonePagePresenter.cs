// MvxFormsWindowsPhonePagePresenter.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// MvvmCross.Forms.Presenter is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com

using MvvmCross.Forms.Presenter.Core;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Controls;
using System;
using Xamarin.Forms;

namespace MvvmCross.Forms.Presenter.WindowsPhone
{
    public class MvxFormsWindowsPhonePagePresenter
        : MvxFormsPagePresenter
        , IMvxPhoneViewPresenter
    {
        private readonly PhoneApplicationFrame _rootFrame;

        public MvxFormsWindowsPhonePagePresenter(PhoneApplicationFrame rootFrame, Application mvxFormsApp)
            : base(mvxFormsApp)
        {
            _rootFrame = rootFrame;
        }

        protected override void CustomPlatformInitialization(NavigationPage mainPage)
        {
            _rootFrame.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}