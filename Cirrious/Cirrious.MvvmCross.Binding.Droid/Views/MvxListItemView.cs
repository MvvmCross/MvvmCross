// MvxListItemView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxListItemView
        : MvxBaseListItemView
        , IMvxListItemView
    {
        private readonly int _templateId;

        public MvxListItemView(Context context, IMvxBindingActivity bindingActivity, int templateId,
                                       object source)
            : base(context, bindingActivity)
        {
            _templateId = templateId;
            Content = BindingActivity.BindingInflate(source, templateId, this);
        }

        #region IMvxListItemView Members

        public int TemplateId
        {
            get { return _templateId; }
        }

        #endregion
    }
}