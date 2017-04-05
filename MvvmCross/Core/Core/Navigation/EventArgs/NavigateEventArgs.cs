using System;
namespace MvvmCross.Core.Navigation.EventArguments
{
    public class NavigateEventArgs : EventArgs
    {
        public NavigateEventArgs()
        {
        }

        public NavigateEventArgs(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        public NavigateEventArgs(string url)
        {
        	Url = url;
        }

        public string Url { get; set; }
        public Type ViewModelType { get; set; }

    }
}
