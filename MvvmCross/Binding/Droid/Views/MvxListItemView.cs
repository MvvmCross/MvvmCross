// MvxListItemView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Views
{
    using System;
    using Android.Content;
    using Android.Runtime;
    using Android.Views;
    using MvvmCross.Binding.Droid.BindingContext;

    [Register("mvvmcross.binding.droid.views.MvxListItemView")]
    public class MvxListItemView
        : MvxBaseListItemView
          , IMvxListItemView
    {
        public MvxListItemView(Context context,
                               IMvxLayoutInflaterHolder layoutInflaterHolder,
                               object dataContext,
		                       ViewGroup parent,
                               int templateId)
			: base(context, layoutInflaterHolder, dataContext, parent)
        {
            TemplateId = templateId;
			Content = AndroidBindingContext.BindingInflate(templateId, parent, false);
            Content.Tag = new ContextObject(AndroidBindingContext);
        }

        public class ContextObject : Java.Lang.Object
        {
            public ContextObject(IMvxAndroidBindingContext bindingContext)
            {
                this.BindingContext = bindingContext;
            }

            public IMvxAndroidBindingContext BindingContext { get; set; }
        }

        protected MvxListItemView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public int TemplateId { get; }
    }
}