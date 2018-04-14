// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public static class MvxFormsExtensions
    {
        public static bool IsViewModelTypeOf(this Page page, Type viewModelType)
        {
            return (page as IMvxPage)?.ViewModel.GetType() == viewModelType;
        }

        public static TElement Build<TElement>(this TElement element, Action<TElement> buildAction)
        {
            if (element != null)
                buildAction?.Invoke(element);
            return element;
        }
    }
}
