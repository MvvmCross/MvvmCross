// GeneralListItemView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Android.Content;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces.Lists;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings;
using Cirrious.MvvmCross.Binding.Droid.Views;
using CrossUI.Droid;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;

namespace Cirrious.MvvmCross.AutoView.Droid.Views.Lists
{
    public class GeneralListItemView
        : MvxBaseListItemView
          , IMvxLayoutListItemView
    {
        private readonly string _templateName;

        public GeneralListItemView(Context context,
                                   IMvxLayoutInflater layoutInflater,
                                   Dictionary<string, string> textBindings,
                                   object source,
                                   string templateName)
            : base(context, layoutInflater, source)
        {
            _templateName = templateName;
            var templateId = GetTemplateId();
            this.BindingInflate(templateId, this);
            BindProperties(textBindings);
#warning Need to sort out the HandleClick stuff?
            //this.Click += HandleClick;
        }

#warning Need to sort out the HandleClick stuff?
        private void HandleClick(object sender, EventArgs e)
        {
            if (_selectedCommand == null)
                return;

            _selectedCommand.Execute(base.DataContext);
        }

        private int GetTemplateId()
        {
            return DroidResources.FindResourceId("listitem_" + _templateName);
        }

        public string UniqueName
        {
            get { return @"General$" + _templateName; }
        }

        private void BindProperties(Dictionary<string, string> textBindings)
        {
            var binder = Mvx.Resolve<IMvxBinder>();
            var list = new List<IMvxUpdateableBinding>();
            foreach (var kvp in textBindings)
            {
                var binding = binder.BindSingle(DataContext, this, kvp.Key, kvp.Value);
                if (binding != null)
                {
                    BindingContext.RegisterBinding(this, binding);
                }
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