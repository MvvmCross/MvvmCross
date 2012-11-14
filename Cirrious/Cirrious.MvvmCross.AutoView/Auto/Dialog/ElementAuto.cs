using System;
using System.Linq.Expressions;
using System.Windows.Input;
using Foobar.Dialog.Core.Descriptions;

namespace Cirrious.MvvmCross.AutoView.Auto.Dialog
{
    // TODO 
    // - Radio, Html, Image, MultilineEntry, StringMultiline, StyledMultiline, View, WebContent

    public class ElementAuto : KeyedAuto
    {
        public string Caption { get; set; }
        public Expression<Func<ICommand>> SelectedCommand { get; set; }
        public string LayoutName { get; set; }

        public ElementAuto(string key, string caption = null, string onlyFor = null, string notFor = null, Expression<Func<ICommand>> selectedCommand = null, string layoutName = null)
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
                toReturn.Properties["Caption"] = Caption;
            if (LayoutName != null)
                toReturn.Properties["LayoutName"] = LayoutName;
            if (SelectedCommand != null)
            {
                var selectedCommand = SelectedCommand.GetPropertyText();
                toReturn.Properties["SelectedCommand"] = string.Format("@MvxBind:{{'Path':'{0}'}}", selectedCommand);
            }

            return toReturn;
        }
    }
}