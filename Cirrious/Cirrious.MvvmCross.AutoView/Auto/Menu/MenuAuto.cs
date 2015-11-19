// MenuAuto.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq.Expressions;
using System.Windows.Input;
using CrossUI.Core.Descriptions;
using CrossUI.Core.Descriptions.Menu;

namespace Cirrious.MvvmCross.AutoView.Auto.Menu
{
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
            Caption = caption;
            LongCaption = longCaption;
            Icon = icon;
            Command = command;
        }

        public override sealed KeyedDescription ToDescription()
        {
            return ToMenuDescription();
        }

        public virtual MenuDescription ToMenuDescription()
        {
            var toReturn = new MenuDescription();
            base.Fill(toReturn);
            if (Caption != null)
                toReturn.Properties["Caption"] = Caption;
            if (LongCaption != null)
                toReturn.Properties["LongCaption"] = LongCaption;
            if (Icon != null)
                toReturn.Properties["Icon"] = Icon;
            if (Command != null)
            {
                var command = Command.GetPropertyText();
                toReturn.Properties["Command"] = $"@MvxBind:{command}";
            }

            return toReturn;
        }
    }
}