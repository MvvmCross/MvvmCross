// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Platform.Core;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views.EventSource
{
    public class MvxEventSourceMasterDetailPage : MasterDetailPage, IMvxEventSourcePage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
            AppearingCalled.Raise(this);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            DisappearingCalled.Raise(this);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContextChangedCalled.Raise(this);
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            ParentSetCalled.Raise(this);
        }

        public event EventHandler AppearingCalled;
        public event EventHandler DisappearingCalled;
        public event EventHandler BindingContextChangedCalled;
        public event EventHandler ParentSetCalled;
    }
}