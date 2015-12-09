// GeneralListItemView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Droid.Views.Lists
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    using Android.Content;
    using Android.Views;
    using Android.Widget;

    using CrossUI.Droid;

    using MvvmCross.AutoView.Droid.Interfaces.Lists;
    using MvvmCross.Platform;

    public class GeneralListItemView
        : MvxBaseListItemView
          , IMvxLayoutListItemView
    {
        private readonly string _templateName;

        public GeneralListItemView(Context context,
                                   IMvxLayoutInflaterHolder layoutInflaterHolder,
                                   Dictionary<string, string> textBindings,
                                   object source,
                                   string templateName)
            : base(context, layoutInflaterHolder, source)
        {
            this._templateName = templateName;
            var templateId = this.GetTemplateId();
            this.BindingInflate(templateId, this);
            this.BindProperties(textBindings);
#warning Need to sort out the HandleClick stuff?
            //this.Click += HandleClick;
        }

#warning Need to sort out the HandleClick stuff?

        private void HandleClick(object sender, EventArgs e)
        {
            this._selectedCommand?.Execute(base.DataContext);
        }

        private int GetTemplateId()
        {
            return DroidResources.FindResourceId("listitem_" + this._templateName);
        }

        public string UniqueName => @"General$" + this._templateName;

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
            get { return this.GetTextFor("title"); }
            set { this.SetTextFor("title", value); }
        }

        public string SubTitle
        {
            get { return this.GetTextFor("subtitle"); }
            set { this.SetTextFor("subtitle", value); }
        }

        private ICommand _selectedCommand;

        public ICommand SelectedCommand
        {
            get { return this._selectedCommand; }
            set { this._selectedCommand = value; }
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
            var view = this.FindSubView<TextView>(partName);
            if (view != null)
            {
                view.Text = value;
            }
        }

        private string GetTextFor(string partName)
        {
            var view = this.FindSubView<TextView>(partName);
            return view?.Text;
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