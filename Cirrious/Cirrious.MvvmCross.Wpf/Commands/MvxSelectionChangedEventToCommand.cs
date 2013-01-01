﻿// MvxSelectionChangedEventToCommand.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Windows.Controls;
using Cirrious.MvvmCross.Commands;

namespace Cirrious.MvvmCross.Wpf.Commands
{
    public class MvxSelectionChangedEventToCommand : MvxWithArgsEventToCommand
    {
        protected override object MapCommandParameter(object parameter)
        {
            var selectionChangedEventArgs = parameter as SelectionChangedEventArgs;
            if (selectionChangedEventArgs == null)
                return parameter;

            return new MvxSimpleSelectionChangedEventArgs
                {
                    AddedItems = selectionChangedEventArgs.AddedItems,
                    RemovedItems = selectionChangedEventArgs.RemovedItems
                };
        }
    }
}