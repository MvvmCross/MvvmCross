// MvxViewForAttribute.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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