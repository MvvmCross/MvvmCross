using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cirrious.MonoCross.Extensions.Interfaces;

namespace Cirrious.MonoCross.Extensions.ViewModel
{
    public abstract class MXPropertyNotifyBase : INotifyPropertyChanged
    {
        public IMXViewDispatcher ViewDispatcher { get; set; }

        protected void FirePropertyChanged(string whichProperty)
        {
            if (ViewDispatcher != null)
                ViewDispatcher.RequestMainThreadAction(
                    () =>
                        {
                            if (PropertyChanged != null)
                                PropertyChanged(this, new PropertyChangedEventArgs(whichProperty));
                        });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class MXViewModelBase : MXPropertyNotifyBase, IMXViewModel 
    {
        // TODO - how to inject this?

        protected MXViewModelBase()
        {
            // nothing to do currently
        }

        public virtual void RequestStop()
        {
            // default behaviour does nothing!
        }

        protected bool RequestMainThreadAction(Action action)
        {
            if (ViewDispatcher != null)
                return ViewDispatcher.RequestMainThreadAction(action);

            return false;
        }

        protected bool RequestNavigate(string url, Dictionary<string, string> parameters)
        {
            if (ViewDispatcher != null)
                return ViewDispatcher.RequestNavigate(url, parameters);

            return false;
        }
    }
}