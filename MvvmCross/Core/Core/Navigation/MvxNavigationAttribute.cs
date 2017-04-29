using System;

namespace MvvmCross.Core.Navigation
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class MvxNavigationAttribute : Attribute
    {
        public MvxNavigationAttribute(Type viewModelOrFacade, string uriRegex)
        {
            ViewModelOrFacade = viewModelOrFacade;
            UriRegex = uriRegex;
        }

        public Type ViewModelOrFacade { get; }

        public string UriRegex { get; }
    }
}