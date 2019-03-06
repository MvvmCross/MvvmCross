// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Forms.Views;
using MvvmCross.ViewModels;
using Xamarin.Forms;

namespace MvvmCross.Forms.Core
{
    public class MvxFormsApplication : Application
    {
        protected MvxFormsApplication()
        {
            ModalPopped += OnModalPopped;
        }

        public event EventHandler Start;

        public event EventHandler Sleep;

        public event EventHandler Resume;

        protected override void OnStart()
        {
            Start?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnSleep()
        {
            Sleep?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnResume()
        {
            Resume?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnPropertyChanging(string propertyName = null)
        {
            if (propertyName == nameof(MainPage)
                && MainPage is IMvxPage mvxPage
                && mvxPage.ViewModel is IMvxViewModel mvxViewModel)
                mvxViewModel.ViewDestroy(true);

            base.OnPropertyChanging(propertyName);
        }

        private void OnModalPopped(object sender, ModalPoppedEventArgs e)
        {
            if (e.Modal is IMvxPage mvxPage && mvxPage.ViewModel is IMvxViewModel modalViewModel)
                modalViewModel.ViewDestroy(true);
        }
    }
}
