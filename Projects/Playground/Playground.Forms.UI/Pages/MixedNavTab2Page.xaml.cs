// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Playground.Core.ViewModels;
using Xamarin.Forms.Xaml;

namespace Playground.Forms.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxTabbedPagePresentation(WrapInNavigationPage = false, Title = "Tab2")]
    public partial class MixedNavTab2Page : MvxContentPage<MixedNavTab2ViewModel>
    {
        public MixedNavTab2Page()
        {
            InitializeComponent();
        }
    }
}
