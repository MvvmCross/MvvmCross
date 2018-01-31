// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;
using Xamarin.Forms;

namespace Playground.Forms.Pages
{
    [MvxTabbedPagePresentation(WrapInNavigationPage = false)]
    public partial class Tab3Page : MvxContentPage<Tab3ViewModel>
    {
        public Tab3Page()
        {
            InitializeComponent();
        }
    }
}
