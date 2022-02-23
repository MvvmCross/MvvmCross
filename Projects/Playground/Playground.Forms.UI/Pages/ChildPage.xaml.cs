// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Playground.Core.ViewModels;
using Xamarin.Forms;

namespace Playground.Forms.UI.Pages
{
    [MvxContentPagePresentation(WrapInNavigationPage = true, NoHistory = false)]
    public partial class ChildPage : MvxContentPage<ChildViewModel>
    {
        public ChildPage()
        {
            InitializeComponent();

            BrokenTextLabel.PropertyChanged += BrokenTextLabel_PropertyChanged;
        }

        private void BrokenTextLabel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // This demonstrates how MvxFullBinding captures UI exception
            if (e.PropertyName == nameof(BrokenTextLabel.Text))
                throw new NotImplementedException();
        }
    }
}
