using System;
using System.Collections.Generic;
using MvvmCross.Commands;

namespace MvvmCross.Tests
{
    public class MvxUnitTestCommandHelper : IMvxCommandHelper
    {
        public event EventHandler CanExecuteChanged;

        private Dictionary<object, int> items = new Dictionary<object, int>();

        public void WillCallRaisePropertyChangedFor(object item)
        {
            items.Add(item, 0);
        }

        public bool HasCalledRaisePropertyChangedFor(object item)
        {
            try
            {
                return items[item] > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void RaiseCanExecuteChanged(object sender)
        {
            try
            {
                items[sender] += 1;
            }
            catch (Exception)
            {
            }

            if (CanExecuteChanged != null)
            {
                CanExecuteChanged.Invoke(sender, new System.EventArgs());
            }
        }
    }
}
