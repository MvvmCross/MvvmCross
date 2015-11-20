// MvxSingleSelectionChangedEventToCommand.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Windows.Controls;

namespace Cirrious.MvvmCross.WindowsPhone.Commands
{
    public class MvxSingleSelectionChangedEventToCommand : MvxWithArgsEventToCommand
    {
        protected override object MapCommandParameter(object parameter)
        {
            var selectionChangedEventArgs = parameter as SelectionChangedEventArgs;
            if (selectionChangedEventArgs == null)
                return parameter;

            if (selectionChangedEventArgs.AddedItems.Count > 0)
                return selectionChangedEventArgs.AddedItems[0];

            return null;
        }
    }
}