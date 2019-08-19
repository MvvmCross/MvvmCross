﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Forms.Views;
using MvvmCross.Forms.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Forms.UI.Pages
{
    [MvxTabbedPagePresentation(WrapInNavigationPage = true, HostViewModelType = typeof(Tab1ViewModel))]
    public partial class Tab3Page : MvxContentPage<Tab3ViewModel>
    {
        #region Public Constructors

        public Tab3Page()
        {
            InitializeComponent();
        }

        #endregion Public Constructors
    }
}
