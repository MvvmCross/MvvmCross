using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Playground.Core.ViewModels;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.BindingContext;
using Android.Graphics.Drawables;
using Android.Graphics;

namespace Playground.Droid.Views
{
    [MvxFragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame, true,
                         Resource.Animation.abc_fade_in,
                         Resource.Animation.abc_fade_out,
                         Resource.Animation.abc_fade_in,
                         Resource.Animation.abc_fade_out)]
    [MvxFragmentPresentation(typeof(SplitRootViewModel), Resource.Id.split_content_frame)]
    [MvxFragmentPresentation(typeof(TabsRootViewModel), Resource.Id.content_frame)]
    [MvxFragmentPresentation(fragmentHostViewType: typeof(ModalNavView), fragmentContentId: Resource.Id.dialog_content_frame)]
    [Register(nameof(DictionaryBindingView))]
    public class DictionaryBindingView : MvxFragment<DictionaryBindingViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.dictionary_view, null);
            var background = view.FindViewById<LinearLayout>(Resource.Id.container);

            this.CreateBindingSet<DictionaryBindingView, DictionaryBindingViewModel>().Bind(background).For(v => v.Background).To(vm => vm.Value)
                .WithDictionaryConversion(new Dictionary<int, Drawable>
                {
                    {0, new ColorDrawable(Color.Blue) },
                    {1, new ColorDrawable(Color.Red) },
                    {2, new ColorDrawable(Color.Yellow) },
                    {3, new ColorDrawable(Color.Violet) }
                }).Apply();

            return view;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}