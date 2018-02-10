// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Windows.Controls;

namespace MvvmCross.Platform.Wpf.Commands
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
