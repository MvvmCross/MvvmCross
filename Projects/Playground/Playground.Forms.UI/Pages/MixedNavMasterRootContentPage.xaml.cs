﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Forms.Views;
using MvvmCross.Forms.Presenters.Attributes;
using Playground.Core.ViewModels;
using Xamarin.Forms.Xaml;

namespace Playground.Forms.UI.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, WrapInNavigationPage = true, NoHistory = true)]
    public partial class MixedNavMasterRootContentPage : MvxContentPage<MixedNavMasterRootContentViewModel>
	{
		public MixedNavMasterRootContentPage ()
		{
			InitializeComponent ();
		}
	}
}
