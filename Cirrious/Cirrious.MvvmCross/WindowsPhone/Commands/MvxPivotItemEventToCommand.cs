#region Copyright
// <copyright file="MvxSelectionChangedEventToCommand.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Commands;
using Microsoft.Phone.Controls;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.WindowsPhone.Commands
{
    public class MvxPivotItemEventToCommand : MvxWithArgsEventToCommand 
    {
        protected override object MapCommandParameter(object parameter)
        {
            var selectionChangedEventArgs = parameter as PivotItemEventArgs;
            if (selectionChangedEventArgs == null)
                return parameter;

            return new MvxPivotItemChangedEventArgs()
                       {
                           PivotItemName = selectionChangedEventArgs.Item.Tag as string
                       };
        }
    }
}
