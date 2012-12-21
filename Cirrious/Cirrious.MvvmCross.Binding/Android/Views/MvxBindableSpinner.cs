using System.Collections;
using Android.Content;
using Android.Util;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace Cirrious.MvvmCross.Binding.Android.Views
{
    public class MvxBindableSpinner : Spinner
    {
        public MvxBindableSpinner(Context context, IAttributeSet attrs)
            : this(context, attrs, new MvxBindableListAdapter(context) { SimpleViewLayoutId = global::Android.Resource.Layout.SimpleDropDownItem1Line })
        {
        }

        public MvxBindableSpinner(Context context, IAttributeSet attrs, MvxBindableListAdapter adapter)
            : base(context, attrs)
        {
            var itemTemplateId = MvxBindableListViewHelpers.ReadAttributeValue(context, attrs, MvxAndroidBindingResource.Instance.BindableListViewStylableGroupId, MvxAndroidBindingResource.Instance.BindableListItemTemplateId);
            var dropDownItemTemplateId = MvxBindableListViewHelpers.ReadAttributeValue(context, attrs, MvxAndroidBindingResource.Instance.BindableListViewStylableGroupId, MvxAndroidBindingResource.Instance.BindableDropDownListItemTemplateId);
            adapter.ItemTemplateId = itemTemplateId;
            adapter.DropDownItemTemplateId = dropDownItemTemplateId;
            Adapter = adapter;
            SetupHandleItemSelected();
        }

        public new MvxBindableListAdapter Adapter
        {
            get { return base.Adapter as MvxBindableListAdapter; }
            set
            {
                var existing = Adapter;
                if (existing == value)
                    return;

                if (existing != null && value != null)
                {
                    value.ItemsSource = existing.ItemsSource;
                    value.ItemTemplateId = existing.ItemTemplateId;
                }

                base.Adapter = value;
            }
        }

        [MvxSetToNullAfterBinding]
        public IList ItemsSource
        {
            get { return Adapter.ItemsSource; }
            set { Adapter.ItemsSource = value; }
        }

        public int ItemTemplateId
        {
            get { return Adapter.ItemTemplateId; }
            set { Adapter.ItemTemplateId = value; }
        }

        public int DropDownItemTemplateId
        {
            get { return Adapter.DropDownItemTemplateId; }
            set { Adapter.DropDownItemTemplateId = value; }
        }

        public IMvxCommand HandleItemSelected { get; set; }

        private void SetupHandleItemSelected()
        {
            base.ItemSelected += (sender, args) =>
                                     {
                                         var item = Adapter.GetRawItem(args.Position);
                                         if (this.HandleItemSelected == null 
                                             || item == null 
                                             || !this.HandleItemSelected.CanExecute(item))
                                             return;

                                         this.HandleItemSelected.Execute(item);
                                     };
        }
    }
}