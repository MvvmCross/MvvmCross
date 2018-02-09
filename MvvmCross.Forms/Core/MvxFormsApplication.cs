// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Xamarin.Forms;

namespace MvvmCross.Forms.Core
{
    public class MvxFormsApplication : Application
    {
        protected MvxFormsApplication()
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
