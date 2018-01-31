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
    [MvxMasterDetailPagePresentation]
    public partial class SplitDetailNavPage : MvxContentPage<SplitDetailNavViewModel>
    {
        public SplitDetailNavPage()
        {
            InitializeComponent();
        }
    }
}
