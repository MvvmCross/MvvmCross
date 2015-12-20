// MvxSelectionChangedEventToCommand.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Wpf.Commands
{
    using System.Windows.Controls;

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