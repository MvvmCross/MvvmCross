// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Forms.Core;
using Xamarin.Forms;

namespace Playground.Forms.UI
{
    public partial class FormsApp : Application
    {
        public FormsApp()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
        }
    }
}
