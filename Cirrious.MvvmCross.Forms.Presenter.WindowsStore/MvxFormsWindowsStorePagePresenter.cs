// MvxFormsWindowsPhonePagePresenter.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// Cirrious.MvvmCross.Forms.Presenter is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com
﻿
﻿using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Xamarin.Forms;
using System;
using Cirrious.MvvmCross.Forms.Presenter.Core;
using Cirrious.MvvmCross.WindowsCommon.Views;

namespace Cirrious.MvvmCross.Forms.Presenter.WindowsPhone
{
    public class MvxFormsWindowsPhonePagePresenter 
        : MvxFormsPagePresenter
        , IMvxWindowsViewPresenter
    {
        private readonly Frame _rootFrame;

        public MvxFormsWindowsPhonePagePresenter(Frame rootFrame, Application mvxFormsApp)
            : base(mvxFormsApp)
        {
            _rootFrame = rootFrame;
        }
    }
}
