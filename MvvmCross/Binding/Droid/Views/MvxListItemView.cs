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

    [Register("mvvmcross.binding.droid.views.MvxListItemView")]
    public class MvxListItemView
        : MvxBaseListItemView
          , IMvxListItemView
    {
        private readonly int _templateId;

        public MvxListItemView(Context context,
                               IMvxLayoutInflaterHolder layoutInflaterHolder,
                               object dataContext,
                               int templateId)
            : this(context, layoutInflaterHolder, dataContext, null, templateId)
        {
        }

        public MvxListItemView(Context context,
                               IMvxLayoutInflaterHolder layoutInflaterHolder,
                               object dataContext,
                               object parentDataContext,
                               int templateId)
            : base(context, layoutInflaterHolder, dataContext, parentDataContext)
        {
            this._templateId = templateId;
            this.AndroidBindingContext.BindingInflate(templateId, this);
        }

        protected MvxListItemView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public int TemplateId => this._templateId;
    }
}