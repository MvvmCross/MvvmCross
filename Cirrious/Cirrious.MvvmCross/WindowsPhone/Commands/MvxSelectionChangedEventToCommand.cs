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
#warning Really need to put in a credit here to mvvmlight!

using System.Windows.Controls;
using Cirrious.MvvmCross.Commands;

namespace Cirrious.MvvmCross.WindowsPhone.Commands
{
    public class MvxSelectionChangedEventToCommand : MvxWithArgsEventToCommand 
    {
        protected override object MapCommandParameter(object parameter)
        {
            var selectionChangedEventArgs = parameter as SelectionChangedEventArgs;
            if (selectionChangedEventArgs == null)
                return parameter;

            return new MvxSimpleSelectionChangedEventArgs()
                       {
                           AddedItems = selectionChangedEventArgs.AddedItems,
                           RemovedItems = selectionChangedEventArgs.RemovedItems
                       };
        }
    }
}
