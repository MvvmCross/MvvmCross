#warning Really need to put in a credit here to mvvmlight!

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using Cirrious.MvvmCross.Commands;

namespace Cirrious.MvvmCross.WindowsPhone.Commands
{
    public class MvxWithArgsEventToCommand : MvxEventToCommand 
    {
        public MvxWithArgsEventToCommand()
        {
            PassEventArgsToCommand = true;
        }
    }
}
