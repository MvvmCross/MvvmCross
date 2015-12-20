// ElementAuto.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Auto.Dialog
{
    using System;
    using System.Linq.Expressions;
    using System.Windows.Input;

    using CrossUI.Core.Descriptions;
    using CrossUI.Core.Descriptions.Dialog;

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
            this.Caption = caption;
            this.SelectedCommand = selectedCommand;
            this.LayoutName = layoutName;
        }

        public sealed override KeyedDescription ToDescription()
        {
            return this.ToElementDescription();
        }

        public virtual ElementDescription ToElementDescription()
        {
            var toReturn = new ElementDescription();
            base.Fill(toReturn);

            if (this.Caption != null)
            {
                toReturn.Properties["Caption"] = this.Caption;
            }

            if (this.LayoutName != null)
            {
                toReturn.Properties["LayoutName"] = this.LayoutName;
            }

            string selectedCommandName = null;
            if (this.SelectedCommandNameOverride != null)
            {
                selectedCommandName = this.SelectedCommandNameOverride;
            }
            else if (this.SelectedCommand != null)
            {
                selectedCommandName = this.SelectedCommand.GetPropertyText();
            }

            if (selectedCommandName != null)
            {
                toReturn.Properties["SelectedCommand"] = $"@MvxBind:{selectedCommandName}";
            }

            return toReturn;
        }
    }
}