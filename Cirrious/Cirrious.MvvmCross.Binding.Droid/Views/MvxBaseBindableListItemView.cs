// MvxBaseBindableListItemView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Droid.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxBaseBindableListItemView
        : FrameLayout
    {
        private readonly IMvxBindingActivity _bindingActivity;

        public MvxBaseBindableListItemView(Context context, IMvxBindingActivity bindingActivity)
            : base(context)
        {
            _bindingActivity = bindingActivity;
        }

        public void ClearBindings()
        {
            _bindingActivity.ClearBindings(this);
        }

        protected IMvxBindingActivity BindingActivity
        {
            get { return _bindingActivity; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearBindings();
            }

            base.Dispose(disposing);
        }

        protected View Content { get; set; }

        public virtual void BindTo(object source)
        {
            BindViewTo(Content, source);
        }

        protected static void BindViewTo(View view, object source)
        {
            IDictionary<View, IList<IMvxUpdateableBinding>> bindings;
            if (!TryGetJavaBindingContainer(view, out bindings))
            {
                return;
            }

            foreach (var binding in bindings)
            {
                foreach (var bind in binding.Value)
                {
                    bind.DataContext = source;
                }
            }
        }

        private static bool TryGetJavaBindingContainer(View view,
                                                       out IDictionary<View, IList<IMvxUpdateableBinding>> result)
        {
            return view.TryGetStoredBindings(out result);
        }
    }
}