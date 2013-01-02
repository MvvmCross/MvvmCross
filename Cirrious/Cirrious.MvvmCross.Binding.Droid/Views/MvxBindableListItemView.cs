// MvxBindableListItemView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxBindableListItemView
        : MvxBaseBindableListItemView
          , IMvxBindableListItemView
    {
        private readonly int _templateId;

        public MvxBindableListItemView(Context context, IMvxBindingActivity bindingActivity, int templateId,
                                       object source)
            : base(context, bindingActivity)
        {
            _templateId = templateId;
            Content = BindingActivity.BindingInflate(source, templateId, this);
        }

        #region IMvxBindableListItemView Members

        public int TemplateId
        {
            get { return _templateId; }
        }

        #endregion
    }
}