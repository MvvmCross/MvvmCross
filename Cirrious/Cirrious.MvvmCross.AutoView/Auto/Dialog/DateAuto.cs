#region Copyright

// <copyright file="DateAuto.cs" company="Cirrious">
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

namespace Cirrious.MvvmCross.AutoView.Auto.Dialog
{
    public class DateAuto : ValueElementAuto
    {
        public DateAuto(string key = null, Expression<Func<object>> bindingExpression = null, string converter = null,
                        string converterParameter = null, string value = null, string caption = null,
                        string onlyFor = null, string notFor = null, Expression<Func<ICommand>> selectedCommand = null,
                        string layoutName = null)
            : base(
                key ?? "Date", bindingExpression, converter, converterParameter, value, caption, onlyFor, notFor,
                selectedCommand, layoutName)
        {
        }
    }
}