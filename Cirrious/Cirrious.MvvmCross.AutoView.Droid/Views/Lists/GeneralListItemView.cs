using System;
using System.Collections.Generic;
using System.Windows.Input;
using Android.Content;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces.Lists;
using Cirrious.MvvmCross.Binding.Droid.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using CrossUI.Droid;

namespace Cirrious.MvvmCross.AutoView.Droid.Views.Lists
{
    public class GeneralListItemView
        : MvxBaseBindableListItemView
        , IMvxLayoutListItemView
        , IMvxServiceConsumer
    {
        private readonly string _templateName;
        private object _dataContext;

        public GeneralListItemView(Context context, IMvxBindingActivity bindingActivity, string templateName, object source) 
            : base(context, bindingActivity)
        {
            _templateName = templateName;
            var templateId = GetTemplateId();
            Content = BindingActivity.BindingInflate(source, templateId, this);
#warning Need to sort out the HandleClick stuff?
            //this.Click += HandleClick;
        }

        private void HandleClick(object sender, EventArgs e)
        {
            if (_selectedCommand == null)
                return;

            _selectedCommand.Execute(_dataContext);
        }

        private int GetTemplateId()
        {
            return DroidResources.FindResourceId("listitem_" + _templateName);
        }

        public string UniqueName { get { return @"General$" + _templateName; } }

        public override void BindTo(object source)
        {
            _dataContext = source;
            BindViewTo(this, source);
            base.BindTo(source);
        }

        public void BindProperties(object source, Dictionary<string, string> textBindings)
        {
            var binder = this.GetService<IMvxBinder>();
            var list = new List<IMvxUpdateableBinding>();
            foreach (var kvp in textBindings)
            {
                var binding = binder.BindSingle(source, this, kvp.Key, kvp.Value);
                if (binding != null)
                {
                    list.Add(binding);
                }
            }
            if (list.Count > 0)
            {
                this.StoreBindings(list);                
            }
        }


        public string Title
        {
            get { return GetTextFor("title"); }
            set { SetTextFor("title", value); }
        }

        public string SubTitle
        {
            get { return GetTextFor("subtitle"); }
            set { SetTextFor("subtitle", value); }
        }

        private ICommand _selectedCommand;
        public ICommand SelectedCommand
        {
            get { return _selectedCommand; }
            set { _selectedCommand = value; }
        }

        /*
         * TODO!
        public string ImageUrl
        {
            get { }
            set { }
        }
        // TODO - bindable properties for:
        // Title
        // SubTitle
        // Image
        // Click
        // LongClick?

        // later:
        // TitleColor
        // SubTitleColor
        // BackgroundColor
        */

        private void SetTextFor(string partName, string value)
        {
            var view = FindSubView<TextView>(partName);
            if (view != null)
            {
                view.Text = value;
            }
        }

        private string GetTextFor(string partName)
        {
            var view = FindSubView<TextView>(partName);
            if (view == null)
            {
                return null;
            }
            return view.Text;
        }

        private TView FindSubView<TView>(string partName) where TView : View 
        {
            var id = Context.Resources.GetIdentifier(GetLayoutName(partName), "id", Context.PackageName);
            if (id == 0)
                return null;
            return FindViewById<TView>(id);
        }

        private static string GetLayoutName(string partName)
        {
            return "listitempart_" + partName;
        }
    }
}