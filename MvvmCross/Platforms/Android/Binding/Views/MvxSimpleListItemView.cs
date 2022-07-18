// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MvvmCross.Platforms.Android.Binding.Views
{
    [Register("mvvmcross.platforms.android.binding.views.MvxSimpleListItemView")]
    public class MvxSimpleListItemView : MvxListItemView
    {
        public MvxSimpleListItemView(Context context, IMvxLayoutInflaterHolder layoutInflaterHolder,
            object dataContext, ViewGroup parent, int templateId)
            : base(context, layoutInflaterHolder, dataContext, parent, templateId)
        {
        }

        public override object DataContext
        {
            get => base.DataContext;
            set
            {
                var context = base.DataContext = value;
                var textView = Content as TextView;
                if (textView != null)
                    textView.Text = context?.ToString();
            }
        }
    }
}
