// GeneralListItemViewFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Droid.Views.Lists
{
    using System.Collections.Generic;

    using Android.Content;
    using Android.Views;

    using MvvmCross.AutoView.Droid.Interfaces.Lists;

    public class GeneralListItemViewFactory
        : IMvxLayoutListItemViewFactory

    {
        public View BuildView(Context context, IMvxAndroidBindingContext androidBindingContext, object source)
        {
            var view = new GeneralListItemView(
                context,
                androidBindingContext.LayoutInflaterHolder,
                this.Bindings,
                source,
                this.LayoutName);
            return view;
        }

        public string UniqueName => @"General$" + this.LayoutName;

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