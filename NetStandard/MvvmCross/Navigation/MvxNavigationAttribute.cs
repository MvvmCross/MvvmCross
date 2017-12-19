﻿using System;

namespace MvvmCross.Core.Navigation
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class MvxNavigationAttribute : Attribute
    {
        public Type ViewModelOrFacade { get; private set; }
        
        public string UriRegex { get; private set; }

        public MvxNavigationAttribute(Type viewModelOrFacade, string uriRegex)
        {
            ViewModelOrFacade = viewModelOrFacade;
            UriRegex = uriRegex;
        }
    }
}
