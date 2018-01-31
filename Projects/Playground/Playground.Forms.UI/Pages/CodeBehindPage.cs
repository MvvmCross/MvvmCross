// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Forms.Views;
using Xamarin.Forms;
using MvvmCross.Binding.BindingContext;
using Playground.Core.ViewModels;

namespace Playground.Forms.Pages
{
    public class CodeBehindPage : MvxContentPage<CodeBehindViewModel>
    {
        private Label _label = new Label();
        private Entry _entry = new Entry();
        public CodeBehindPage()
        {
            Content = new StackLayout
            {
                Children = {
                    _label,
                    _entry
                }
            };

            var set = this.CreateBindingSet<CodeBehindPage, CodeBehindViewModel>();
            set.Bind(_label).For(l => l.Text).To(vm => vm.BindableText);
            set.Bind(_entry).For(e => e.Text).To(vm => vm.BindableText);
            set.Apply();
        }
    }
}