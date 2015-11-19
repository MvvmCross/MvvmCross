// EntryAuto.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Core.Descriptions.Dialog;
using System;
using System.Linq.Expressions;
using System.Windows.Input;

namespace Cirrious.MvvmCross.AutoView.Auto.Dialog
{
    public class EntryAuto : ValueElementAuto
    {
        public bool IsEmail { get; set; }
        public bool Numeric { get; set; }
        public bool Password { get; set; }
        public bool NoSpellCheck { get; set; }

        public EntryAuto(string key = null, Expression<Func<object>> bindingExpression = null, string converter = null,
                         string converterParameter = null, string value = null, string caption = null,
                         string onlyFor = null, string notFor = null, Expression<Func<ICommand>> selectedCommand = null,
                         string layoutName = null)
            : base(
                key ?? "Entry", bindingExpression, converter, converterParameter, value, caption, onlyFor, notFor,
                selectedCommand, layoutName)
        {
        }

        public override ElementDescription ToElementDescription()
        {
            var toReturn = base.ToElementDescription();
            if (IsEmail)
                toReturn.Properties["IsEmail"] = IsEmail;
            if (Password)
                toReturn.Properties["Password"] = Password;
            if (Numeric)
                toReturn.Properties["Numeric"] = Numeric;
            if (NoSpellCheck)
                toReturn.Properties["NoSpellCheck"] = NoSpellCheck;
            return toReturn;
        }
    }
}