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

using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Android.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Android.Views
{
    public class MvxBindableListItemView
        : FrameLayout
        , IMvxBindableListItemView
    {
        private readonly View _content;

        private readonly int _templateId;

        public MvxBindableListItemView(Context context, IMvxBindingActivity bindingActivity, int templateId, object source)
            : base(context)
        {
            _templateId = templateId;
            _content = bindingActivity.BindingInflate(source, templateId, this);
        }

        protected View Content { get { return _content; } }

        #region IMvxBindableListItemView Members

        public int TemplateId
        {
            get { return _templateId; }
        }

        public virtual void BindTo(object source)
        {
            if (_content == null)
                return;

            var tag = _content.GetTag(MvxAndroidBindingResource.Instance.BindingTagUnique);
            if (tag == null)
                return;

            var cast = tag as MvxJavaContainer<Dictionary<View, IList<IMvxUpdateableBinding>>>;
            if (cast == null)
                return;
            
            foreach (var binding in cast.Object)
            {
                foreach (var bind in binding.Value)
                {
                    bind.DataContext = source;
                }
            }
        }

        #endregion
    }
}