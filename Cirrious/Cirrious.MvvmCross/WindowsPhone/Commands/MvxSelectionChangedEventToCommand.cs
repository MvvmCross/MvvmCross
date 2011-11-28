#warning Really need to put in a credit here to mvvmlight!

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
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
