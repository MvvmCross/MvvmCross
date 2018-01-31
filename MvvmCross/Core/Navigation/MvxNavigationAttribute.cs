// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

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
