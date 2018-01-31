// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Core.ViewModels
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MvxViewForAttribute : Attribute
    {
        public Type ViewModel { get; set; }

        public MvxViewForAttribute(Type viewModel)
        {
            ViewModel = viewModel;
        }
    }
}