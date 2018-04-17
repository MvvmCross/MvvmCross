// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using Playground.Core.ViewModels;
using System.Collections.Generic;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace Playground.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame, true,
                         Resource.Animation.abc_fade_in,
                         Resource.Animation.abc_fade_out,
                         Resource.Animation.abc_fade_in,
                         Resource.Animation.abc_fade_out)]
    [Register(nameof(DictionaryBindingView))]
    public class DictionaryBindingView : MvxFragment<DictionaryBindingViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.dictionary_view, null);
            var background = view.FindViewById<LinearLayout>(Resource.Id.container);
            var descriptionLabel = view.FindViewById<TextView>(Resource.Id.txt_description);

            var bindingSet = this.CreateBindingSet<DictionaryBindingView, DictionaryBindingViewModel>();
            bindingSet.Bind(background).For(v => v.Background).To(vm => vm.Value)
                .WithDictionaryConversion(new Dictionary<int, Drawable>
                {
                    [0] = new ColorDrawable(Color.Blue),
                    [1] = new ColorDrawable(Color.Red),
                    [2] = new ColorDrawable(Color.Yellow),
                    [3] = new ColorDrawable(Color.Violet)
                });
            bindingSet.Bind(descriptionLabel).To(vm => vm.Value)
                .WithDictionaryConversion(new Dictionary<int, string>
                {
                    [0] = "Description for blue",
                    [1] = "Description for Red",
                }, "Fallback description");

            bindingSet.Apply();

            return view;
        }
    }
}
