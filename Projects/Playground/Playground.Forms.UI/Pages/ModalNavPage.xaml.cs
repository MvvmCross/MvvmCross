﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Forms.Views;
using MvvmCross.Forms.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Forms.UI.Pages
{
    [MvxModalPresentation(WrapInNavigationPage = true)]
    public partial class ModalNavPage : MvxContentPage<ModalNavViewModel>
    {
        public ModalNavPage()
        {
            InitializeComponent();
        }
    }
}
