// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Playground.Core.ViewModels;

namespace Playground.Forms.UI.Pages
{
    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class SecondChildPage : MvxContentPage<SecondChildViewModel>
    {
        public SecondChildPage()
        {
            InitializeComponent();
        }
    }
}
