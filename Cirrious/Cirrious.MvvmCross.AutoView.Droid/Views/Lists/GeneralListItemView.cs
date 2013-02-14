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
using Cirrious.MvvmCross.AutoView.Droid.Interfaces.Lists;
using Cirrious.MvvmCross.Binding.Droid.Binders;
using Cirrious.MvvmCross.Binding.Droid.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using CrossUI.Droid;

namespace Cirrious.MvvmCross.AutoView.Droid.Views.Lists
{
    public class GeneralListItemView
        : FrameLayout
          , IMvxLayoutListItemView
          , IMvxServiceConsumer
    {
        private readonly string _templateName;
        private object _dataContext;

        public GeneralListItemView(Context context, string templateName, Dictionary<string, string> textBindings)
            : base(context)
        {
            _templateName = templateName;
            var templateId = GetTemplateId();
            LayoutInflater.FromContext(context).Inflate(templateId, this);

            CreateBindings (textBindings);
        }

        private int GetTemplateId()
        {
            return DroidResources.FindResourceId("listitem_" + _templateName);
        }

        public string UniqueName
        {
            get { return @"General$" + _templateName; }
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

        private void CreateBindings(Dictionary<string, string> textBindings) {
            foreach (var kvp in textBindings) {
                bool created = false;

                if ("Title".Equals(kvp.Key)) {
                    created = TryCreateBinding (FindSubView<View>("title"), "Text", kvp.Value);
                } else if ("SubTitle".Equals(kvp.Key)) {
                    created = TryCreateBinding (FindSubView<View>("subtitle"), "Text", kvp.Value);
                } else if ("SelectedCommand".Equals(kvp.Key)) {
                    created = TryCreateBinding (this, "Click", kvp.Value);
                } else {
                    MvxAutoViewTrace.Trace(
                        MvxTraceLevel.Warning,
                        "Unsupported field {0} for a ListItemView.",
                        kvp.Key);
                    continue;
                }

                if (!created) {
                    MvxAutoViewTrace.Trace(
                        MvxTraceLevel.Error,
                        "Failed to bind property {0} for ListItemView, see previous line for details.",
                        kvp.Key);
                }
            }
        }

        private bool TryCreateBinding(View view, String target, String partialBindingDescription) {
            if (view == null)
                return false;

            var desc = this.GetService<IMvxBindingDescriptionParser>().ParseSingle(partialBindingDescription);
            if (desc == null)
                return false;

            desc.TargetName = target;

            return TryAddViewBinding (view, desc);
        }

        private bool TryAddViewBinding(View view, MvxBindingDescription bindingDescription) {
            List<MvxBindingDescription> descriptions = null;
            object dataSource = null;
            bool overrideDataSource = false;
            bool bindingsEnabled = true;

            // Adjust bindings for the tag
            var tag = view.GetBindingTag ();
            if (tag != null) {
                if (tag.Bindings != null && tag.Bindings.Count > 0) {
                    MvxAutoViewTrace.Trace(
                        MvxTraceLevel.Warning,
                        "Cannot modify binding descriptions for already bound views.");
                    return false;
                }

                descriptions = new List<MvxBindingDescription> (tag.BindingDescriptions);
                dataSource = tag.DataSource;
                overrideDataSource = tag.OverrideDataSource;
                bindingsEnabled = tag.BindingEnabled;
            } else {
                descriptions = new List<MvxBindingDescription>(1);
            }

            // See if we need to replace an item:
            bool added = false;
            for (int i = 0; i < descriptions.Count; i++) {
                if (descriptions[i].TargetName == bindingDescription.TargetName) {
                    descriptions[i] = bindingDescription;
                    added = true;
                    break;
                }
            }
            if (!added) {
                descriptions.Add(bindingDescription);
            }

            // Replace tag:
            view.SetBindingTag (new MvxViewBindingTag (descriptions) {
                BindingEnabled = bindingsEnabled,
                DataSource = dataSource,
                OverrideDataSource = overrideDataSource,
            });
            
            return true;
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