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
        protected View Content { get { return _content; } }

        private readonly int _templateId;

        public int TemplateId
        {
            get { return _templateId; }
        }

        public MvxBindableListItemView(Context context, IMvxBindingActivity bindingActivity, int templateId, object source)
            : base(context)
        {
            _templateId = templateId;
            _content = bindingActivity.BindingInflate(source, templateId, this);
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
    }
}