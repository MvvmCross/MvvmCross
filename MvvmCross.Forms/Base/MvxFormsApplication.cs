// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Forms.Views;
using Xamarin.Forms;

namespace MvvmCross.Forms.Base
{
    public class MvxFormsApplication : Application
    {
        public MvxFormsApplication()
        {
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
    }
}
