#region Copyright
// <copyright file="MvxPivotItemEventToCommand.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Commands;
using Microsoft.Phone.Controls;

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
