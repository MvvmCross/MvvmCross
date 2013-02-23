// MvxLayoutDrivenAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces.Lists;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace Cirrious.MvvmCross.AutoView.Droid.Views.Lists
{
    public class MvxLayoutDrivenAdapter : MvxAdapter
    {
        protected readonly Dictionary<string, IMvxLayoutListItemViewFactory> _itemLayouts;
        protected readonly IMvxLayoutListItemViewFactory _defaultItemLayout;

        public MvxLayoutDrivenAdapter(Context context, IMvxLayoutListItemViewFactory defaultItemLayout,
                                          Dictionary<string, IMvxLayoutListItemViewFactory> itemLayouts)
            : base(context)
        {
            _defaultItemLayout = defaultItemLayout;
            _itemLayouts = itemLayouts;
        }

        protected override View GetBindableView(View convertView, object source, int templateId)
        {
            IMvxLayoutListItemViewFactory layout;
            if (source == null)
            {
                layout = _defaultItemLayout;
            }
            else
            {
                if (!_itemLayouts.TryGetValue(source.GetType().Name, out layout))
                {
                    layout = _defaultItemLayout;
                }
            }

            var existing = convertView as IMvxLayoutListItemView;
            if (existing != null)
            {
                if (existing.UniqueName == layout.UniqueName)
                {
                    // reuse the convertView...
                    existing.BindTo(source);
                    return convertView;
                }
            }

            // use a special engine thing
            var view = layout.BuildView(Context, BindingContext, source);
            return view;
        }

        protected override MvxListItemView CreateBindableView(object source, int templateId)
        {
            throw new MvxException(@"CreateBindableView should not be called for layout list items");
        }

        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            throw new MvxException(@"GetDropDownView should not be called for layout list items");
        }

        protected override void BindBindableView(object source, IMvxListItemView viewToUse)
        {
            throw new MvxException(
                @"BindBindableView with IMvxListItemView should not be called for layout list items");
        }
    }
}