#region Copyright

// <copyright file="EntryAuto.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Linq.Expressions;
using System.Windows.Input;
using CrossUI.Core.Descriptions.Dialog;

namespace Cirrious.MvvmCross.AutoView.Auto.Dialog
{
    public class EntryAuto : ValueElementAuto
    {
        public bool IsEmail { get; set; }
        public bool Numeric { get; set; }
        public bool Password { get; set; }

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
            return toReturn;
        }
    }
}