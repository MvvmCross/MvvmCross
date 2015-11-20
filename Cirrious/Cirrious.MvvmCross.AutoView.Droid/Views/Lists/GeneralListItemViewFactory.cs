// GeneralListItemViewFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Views;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces.Lists;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.AutoView.Droid.Views.Lists
{
    public class GeneralListItemViewFactory
        : IMvxLayoutListItemViewFactory

    {
        public View BuildView(Context context, IMvxAndroidBindingContext androidBindingContext, object source)
        {
            var view = new GeneralListItemView(
                context,
                androidBindingContext.LayoutInflaterHolder,
                Bindings,
                source,
                LayoutName);
            return view;
        }

        public string UniqueName => @"General$" + LayoutName;

        public string LayoutName { get; set; }

        public Dictionary<string, string> Bindings { get; set; }
        /*
        private object _bindings;
        public object Bindings
        {
            get { return _bindings; }
            set
            {
                MvxTrace.Trace("Bindings is set to {0} - {1}", value.GetType().Name, value.GetType().FullName);
                _bindings = value;
            }
        }
         */
    }
}