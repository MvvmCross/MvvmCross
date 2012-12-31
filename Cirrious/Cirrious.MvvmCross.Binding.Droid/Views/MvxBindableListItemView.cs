#region Copyright

// <copyright file="MvxBindableListItemView.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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