using System;
using System.Collections.Generic;
using System.Linq;
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
            if (item != null && items.Any() && items.ContainsKey(item))
            {
                return items[item] > 0;
            }

            throw new Exception("This command is not registered for tracking RaiseCanExecuteChanged");
        }

        public void RaiseCanExecuteChanged(object item)
        {
            if (items != null && items.Any() && items.ContainsKey(item))
            {
                items[item] += 1;
            }

            if (CanExecuteChanged != null)
            {
                CanExecuteChanged.Invoke(item, new System.EventArgs());
            }
        }
    }
}
