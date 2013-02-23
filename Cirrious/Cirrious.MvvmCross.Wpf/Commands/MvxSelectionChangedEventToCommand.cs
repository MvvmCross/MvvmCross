// MvxSelectionChangedEventToCommand.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections;
using System.Windows.Controls;

namespace Cirrious.MvvmCross.Wpf.Commands
{
    public class MvxSimpleSelectionChangedEventArgs : EventArgs
    {
        public IList AddedItems { get; set; }
        public IList RemovedItems { get; set; }
    }

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