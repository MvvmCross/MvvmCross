// MvxListItemView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxListItemView
        : MvxBaseListItemView
          , IMvxListItemView
    {
        private readonly int _templateId;

        public MvxListItemView(Context context,
                               IMvxLayoutInflater layoutInflater,
                               object dataContext,
                               int templateId)
            : base(context, layoutInflater, dataContext)
        {
            _templateId = templateId;
            Content = AndroidBindingContext.BindingInflate(templateId, this);
        }

        public int TemplateId
        {
            get { return _templateId; }
        }
    }
}