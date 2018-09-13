// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;
using Playground.Core.ViewModels.Bindings;

namespace Playground.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame)]
    public class FluentBindingView : MvxFragment<FluentBindingViewModel>
    {
        EditText _inputText;
        TextView _outputText;
        private IMvxInteraction<bool> _clearBindingInteraction;
        public IMvxInteraction<bool> ClearBindingInteraction
        {
            get => _clearBindingInteraction;
            set
            {
                if (_clearBindingInteraction != null)
                    _clearBindingInteraction.Requested -= OnInteractionRequested;

                _clearBindingInteraction = value;
                _clearBindingInteraction.Requested += OnInteractionRequested;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.FluentBindingView, null);

            _inputText = view.FindViewById<EditText>(Resource.Id.inputText);
            _outputText = view.FindViewById<TextView>(Resource.Id.outputText);
            var toggleButton = view.FindViewById<Button>(Resource.Id.toggleBtn);

            var bindingSet = this.CreateBindingSet<FluentBindingView, FluentBindingViewModel>();
            bindingSet.Bind(toggleButton).For(v => v.BindClick()).To(vm => vm.ClearBindingsCommand);
            bindingSet.Apply();

            BindTextInput();

            return view;
        }

        void BindTextInput()
        {
            var bindingSet = this.CreateBindingSet<FluentBindingView, FluentBindingViewModel>();
            bindingSet.Bind(_inputText).For(v => v.Text).To(vm => vm.TextValue);
            bindingSet.Bind(_outputText).For(v => v.Text).To(vm => vm.TextValue);
            bindingSet.Bind(this).For(v => v.ClearBindingInteraction).To(vm => vm.ClearBindingInteraction);
            bindingSet.ApplyWithClearBindingKey(nameof(FluentBindingView));
        }

        private void OnInteractionRequested(object sender, MvxValueEventArgs<bool> eventArgs)
        {
            if (eventArgs.Value)
                BindTextInput();
            else
                this.ClearBindings(nameof(FluentBindingView));
        }
    }
}
