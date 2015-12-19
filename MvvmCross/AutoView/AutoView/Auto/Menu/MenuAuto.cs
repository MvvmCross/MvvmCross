// MenuAuto.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Auto.Menu
{
    using System;
    using System.Linq.Expressions;
    using System.Windows.Input;

    using CrossUI.Core.Descriptions;
    using CrossUI.Core.Descriptions.Menu;

    public class MenuAuto : KeyedAuto
    {
        public string Caption { get; set; }
        public string LongCaption { get; set; }
        public string Icon { get; set; }
        public Expression<Func<ICommand>> Command { get; set; }

        public MenuAuto(string key = null, string onlyFor = null, string notFor = null, string caption = null,
                        string longCaption = null, string icon = null, Expression<Func<ICommand>> command = null)
            : base(key, onlyFor, notFor)
        {
            this.Caption = caption;
            this.LongCaption = longCaption;
            this.Icon = icon;
            this.Command = command;
        }

        public sealed override KeyedDescription ToDescription()
        {
            return this.ToMenuDescription();
        }

        public virtual MenuDescription ToMenuDescription()
        {
            var toReturn = new MenuDescription();
            base.Fill(toReturn);
            if (this.Caption != null)
                toReturn.Properties["Caption"] = this.Caption;
            if (this.LongCaption != null)
                toReturn.Properties["LongCaption"] = this.LongCaption;
            if (this.Icon != null)
                toReturn.Properties["Icon"] = this.Icon;
            if (this.Command != null)
            {
                var command = this.Command.GetPropertyText();
                toReturn.Properties["Command"] = $"@MvxBind:{command}";
            }

            return toReturn;
        }
    }
}