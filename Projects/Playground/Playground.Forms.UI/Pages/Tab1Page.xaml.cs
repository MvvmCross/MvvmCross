// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Forms.Pages
{
    [MvxTabbedPagePresentation(WrapInNavigationPage = false, Title = "Tab1")]
    public partial class Tab1Page : MvxContentPage<Tab1ViewModel>
    {
        public Tab1Page()
        {
            InitializeComponent();
        }
    }
}
