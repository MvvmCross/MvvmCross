// ElementAuto.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq.Expressions;
using System.Windows.Input;
using CrossUI.Core.Descriptions;
using CrossUI.Core.Descriptions.Dialog;

namespace Cirrious.MvvmCross.AutoView.Auto.Dialog
{
    // TODO 
    // - Radio, Html, Image, MultilineEntry, StringMultiline, StyledMultiline, View, WebContent

    public class ElementAuto : KeyedAuto
    {
        public string Caption { get; set; }
        public Expression<Func<ICommand>> SelectedCommand { get; set; }
        public string SelectedCommandNameOverride { get; set; }
        public string LayoutName { get; set; }

        public ElementAuto(string key, string caption = null, string onlyFor = null, string notFor = null,
                           Expression<Func<ICommand>> selectedCommand = null, string layoutName = null)
            : base(key, onlyFor, notFor)
        {
            Caption = caption;
            SelectedCommand = selectedCommand;
            LayoutName = layoutName;
        }

        public sealed override KeyedDescription ToDescription()
        {
            return ToElementDescription();
        }

        public virtual ElementDescription ToElementDescription()
        {
            var toReturn = new ElementDescription();
            base.Fill(toReturn);

            if (Caption != null)
            {
                toReturn.Properties["Caption"] = Caption;
            }

            if (LayoutName != null)
            {
                toReturn.Properties["LayoutName"] = LayoutName;
            }

            string selectedCommandName = null;
            if (SelectedCommandNameOverride != null)
            {
                selectedCommandName = SelectedCommandNameOverride;
            }
            else if (SelectedCommand != null)
            {
                selectedCommandName = SelectedCommand.GetPropertyText();
            }

            if (selectedCommandName != null)
            {
                toReturn.Properties["SelectedCommand"] = $"@MvxBind:{selectedCommandName}";
            }

            return toReturn;
        }
    }
}