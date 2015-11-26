// MvxFormsApp.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// MvvmCross.Forms.Presenter is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com

using System;
using Xamarin.Forms;

namespace MvvmCross.Forms.Presenter.Core
{
    public class MvxFormsApp : Application
    {
        public event EventHandler Start;

        public event EventHandler Sleep;

        public event EventHandler Resume;

        protected override void OnStart()
        {
            var handler = Start;
            handler?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnSleep()
        {
            var handler = Sleep;
            handler?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnResume()
        {
            var handler = Resume;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}